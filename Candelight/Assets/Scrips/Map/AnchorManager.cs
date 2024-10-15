using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Map
{
    public enum EAnchorDirection
    {
        Forward, Backward, Right, Left
    }

    public enum ERoomSize
    {
        Small, Medium, Large
    }

    enum EColliderOrientation
    {
        Z = 0, X = 2
    }

    //Para comprobar si se puede generar una nueva sala en un lado, se hace un raycast para comprobar si hay hueco libre
    public class AnchorManager : MonoBehaviour
    {
        ARoom _room;
        MapManager _map;
        [SerializeField] EAnchorDirection _direction;
        Vector3 _raycastDirection; //Direccion del rayo
        Vector3 _auxDirLeft; 
        Vector3 _auxDirRight;

        RaycastHit _hit; //Variable en la que se almacena la informacion del raycast si colisiona con algo
        RaycastHit _auxLhit;
        RaycastHit _auxRhit;

        LayerMask _mask;

        bool _active;
        public bool Connected;

        GameObject _connectionMesh;

        private void Awake()
        {
            //Se rota el anclaje segun la direccion para poder trabajar bien en espacio local
            //switch (_direction)
            //{
            //    case EAnchorDirection.Forward:
            //        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            //        break;
            //    case EAnchorDirection.Backward:
            //        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            //        break;
            //    case EAnchorDirection.Left:
            //        transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
            //        break;
            //    case EAnchorDirection.Right:
            //        transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            //        break;
            //   
            //}
            _raycastDirection = transform.forward;
            _auxDirLeft = transform.forward - 0.5f * transform.right;
            _auxDirLeft.Normalize();
            _auxDirRight = transform.forward + 0.5f * transform.right;
            _auxDirRight.Normalize();

            _room = GetComponentInParent<ARoom>();

            _mask = gameObject.layer;
        }

        //==== GENERACION DEL NIVEL ====
        //PRIMERA FASE:
        // Cada habitacion posee anclajes. Los anclajes tienen probabilidad de activarse al instante o ser ralentizados por un delay
        // Los que se activan comprueban si es posible generar una habitacion y de que tipo (segun el espacio disponible frente a ellos)
        // Se genera la habitacion y esta nueva repite el proceso. Se van registrando en el MapManager
        // Si se alcanza el numero maximo de habitaciones, se impide a los anclajes crear mas
        // Si todos los anclajes activados han generado ya su habitacion y no se ha alcanzado el maximo, entonces entran en juego los originalmente ralentizados por el delay
        // Se generan habitaciones hasta alcanzar el maximo
        #region First Stage: Room Generation

        private void Start()
        {
            _map = MapManager.Instance;

            //Aplicamos probabilidad para que genere instantaneamente o espere
            _active = Random.value < 0.5f;
            if (_active && _map.CanCreateRoom()) SpawnRoom();
            else StartCoroutine(DelayedRoomCreation());
        }

        private void Update()
        {
            //Debug:
            //if (_active)
            //{
            //    Debug.DrawRay(transform.position + 0.44f * transform.forward, _raycastDirection * _map.MediumThreshold, Color.red);
            //    Debug.DrawRay(transform.position + 0.44f * transform.forward, _auxDirLeft * _map.MediumThreshold, Color.yellow);
            //    Debug.DrawRay(transform.position + 0.44f * transform.forward, _auxDirRight * _map.MediumThreshold, Color.yellow);
            //    Debug.DrawRay(transform.position + (_map.MediumThreshold * transform.forward - _map.SmallThreshold * transform.right), _map.SmallThreshold * transform.right, Color.blue);
            //}
        }

        /// <summary>
        /// Se aplica un delay antes de que cree la habitacion. Si no puede crearla, se autodestruye
        /// </summary>
        /// <returns></returns>
        IEnumerator DelayedRoomCreation()
        {
            yield return new WaitForSeconds(0.1f);
            if (_map.CanCreateRoom()) SpawnRoom();
            else Destroy(gameObject);
        }


        /// <summary>
        /// Generamos una nueva habitacion a raiz de un anclaje si el espacio lo permite
        /// </summary>
        /// <returns></returns>
        void SpawnRoom()
        {
            //Averiguamos el tamano maximo 
            int maxSize = Physics.Raycast(transform.position + 0.44f * transform.forward, _raycastDirection, out _hit, _map.MediumThreshold, ~_mask) ? _hit.distance < _map.SmallThreshold ? _hit.distance < _map.RoomSeparation ? -1 : 0 : 1 : 2;
            if (Physics.Raycast(transform.position + 0.44f * transform.forward, _auxDirLeft, out _auxLhit, _map.MediumThreshold, ~_mask) && _auxLhit.transform.parent.parent != transform.parent ||
                Physics.Raycast(transform.position + 0.44f * transform.forward, _auxDirRight, out _auxRhit, _map.MediumThreshold, ~_mask) && _auxRhit.transform.parent.parent != transform.parent) maxSize -= 1;
            //Debug.Log($"Colisiona con {_hit.transform.name}");
            if (Physics.CheckSphere(transform.position + _map.MediumThreshold * transform.forward, _map.SmallThreshold)) maxSize = -1;
            if (maxSize == -1) return;

            ERoomSize size = (ERoomSize)Random.Range(0, maxSize + 1);

            //Generamos un tamano segun el maximo calculado y la posicion que le corresponde
            Vector3 localOffset = OffsetChooser(size);

            //Generamos la nueva sala y encontramos su anclaje
            AnchorManager obj = LocateObjectiveAnchor(_map.RegisterNewRoom(_room.GetID(), transform.position + localOffset, size));

            //Suponiendo que existe el anclaje (en caso contrario, llevaria una referencia a si mismo para evitar errores), generamos la malla para la transicion entre ambos
            _connectionMesh = GenerateMesh(obj);
            SetUVToWorld uvScript = _connectionMesh.AddComponent<SetUVToWorld>();
            uvScript.MaterialToUse = _map.ConnectionMat;
            transform.localPosition -= 0.01f * transform.up;
        }

        /// <summary>
        /// Devuelve la colocacion que debera tomar la neuva habitacion con respecto al anclaje que la genera
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        Vector3 OffsetChooser(ERoomSize size)
        {
            switch ((int)size)
            {
                case 2: //Si es grande
                    return (_map.LargeThreshold / 2) * _raycastDirection;
                case 1: //Si es mediano
                    return (_hit.distance > 0.1f ? _hit.distance / 2 : _map.MediumThreshold / 2) * _raycastDirection;
                case 0: //Si es pequeno
                    return (_hit.distance > 0.1f ? _hit.distance / 2 : _map.SmallThreshold / 2) * _raycastDirection;
                default:
                    return new Vector3();
            }
        }

        #endregion

        //SEGUNDA FASE:
        // Tras generar la habitacion, el anclaje busca el anclaje mas cercano de esta y se conecta con el
        // Se genera un LineRenderer que los conecta (de 4 puntos) para posteriormente ser bakeado en mesh
        // Se ajustan las propiedades de la mesh para que se muestre correctamente (la malla esta en world space)
        #region Second Stage: Connections Generation
        /// <summary>
        /// Se localiza el anclaje mas cercano de una habitacion
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        AnchorManager LocateObjectiveAnchor(GameObject room)
        {
            AnchorManager[] anchors = room.GetComponentsInChildren<AnchorManager>();
            float minDist = 9999;
            AnchorManager obj = this; //Pongo this en vez de null para que no de problemas en caso de fallo horrible dios no lo quiera
            foreach (var a in anchors)
            {
                float dist = Vector3.Distance(transform.position, a.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    obj = a;
                }
            }
            return obj;
        }

        /// <summary>
        /// Se genera una linea que conecta con el otro anclaje y se transforma en malla
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        GameObject GenerateMesh(AnchorManager obj)
        {
            GameObject lineGO = new GameObject("Transicion");
            lineGO.transform.parent = transform;
            LineRenderer line = lineGO.AddComponent<LineRenderer>();

            //Se configura la linea que une ambos anclajes para conseguir la forma deseada
            line.positionCount = 4;
            line.widthMultiplier = _map.ConnectionWidth;
            line.numCapVertices = 1;
            line.numCornerVertices = 2;
            Vector3[] positions = new Vector3[4];
            positions[0] = transform.position;
            //Se elige una posicion adecuada y visualmente agradable para los puntos intermedios
            positions[1] = transform.position;
            positions[2] = transform.position;
            if (_direction == EAnchorDirection.Forward || _direction == EAnchorDirection.Backward)
            {
                positions[1] = new Vector3(positions[0].x, 0f, ((transform.position + obj.transform.position) / 2).z);
                positions[2] = new Vector3(obj.transform.position.x, 0f, positions[1].z);
            }
            else
            {
                positions[1] = new Vector3(((transform.position + obj.transform.position) / 2).x, 0f, (positions[0].z));
                positions[2] = new Vector3(positions[1].x, 0f, obj.transform.position.z);
            }
            positions[3] = obj.transform.position;
            line.SetPositions(positions);
            Destroy(obj.gameObject);
            Connected = true;

            GetComponent<Collider>().enabled = false;

            //Se bakea la imagen de la linea en una mesh
            Mesh transitionMesh = new Mesh();
            line.BakeMesh(transitionMesh, _map.ConnectionBakeCam);
            GameObject meshGO = new GameObject("TransitionMesh");
            meshGO.transform.parent = transform;
            MeshRenderer meshData = meshGO.AddComponent<MeshRenderer>();
            MeshFilter mesh = meshGO.AddComponent<MeshFilter>();
            mesh.mesh = transitionMesh;

            line.enabled = false;

            //Colliders
            BoxCollider baseCol = meshGO.AddComponent<BoxCollider>();
            baseCol.center = new Vector3(baseCol.center.x, 0f, baseCol.center.z);

            if (Mathf.Abs(positions[0].x - positions[3].x) < 0.1f && Mathf.Abs(positions[0].z - positions[3].z) > 1f) GenerateSimpleCollidersZ(meshGO, positions);
            else if (Mathf.Abs(positions[0].z - positions[3].z) < 0.1f && Mathf.Abs(positions[0].x - positions[3].x) > 1f) GenerateSimpleCollidersX(meshGO, positions);
                    else GenerateComplexColliders(meshGO, positions, line.endWidth);

            return meshGO;
        }

        #endregion

        //TERCERA FASE:
        // Segun las propiedades de la conexion, se decide si generar colliders simples o complejos
        // Se crean BoxColliders. Se altera su center y size para que se adecuen a su correspondiente conexion
        #region Third Stage: Collider Generation

        void GenerateSimpleCollidersZ(GameObject con, Vector3[] positions)
        {
            BoxCollider box1 = con.AddComponent<BoxCollider>();
            BoxCollider box2 = con.AddComponent<BoxCollider>();
            box1.center += _map.ConnectionCollidersOffset * Vector3.right;
            box2.center -= _map.ConnectionCollidersOffset * Vector3.right;
            box1.size = new Vector3(box1.size.x, 2f, box1.size.z * 0.8f);
            box2.size = new Vector3(box1.size.x, 2f, box1.size.z * 0.8f);
        }

        void GenerateSimpleCollidersX(GameObject con, Vector3[] positions)
        {
            BoxCollider box1 = con.AddComponent<BoxCollider>();
            BoxCollider box2 = con.AddComponent<BoxCollider>();
            box1.center += _map.ConnectionCollidersOffset * Vector3.forward;
            box2.center -= _map.ConnectionCollidersOffset * Vector3.forward;
            box1.size = new Vector3(box1.size.x * 0.8f, 2f, box1.size.z);
            box2.size = new Vector3(box1.size.x * 0.8f, 2f, box1.size.z);
        }

        void GenerateComplexColliders(GameObject con, Vector3[] positions, float lineWidth)
        {
            BoxCollider[] boxes = new BoxCollider[4];

            boxes[0] = con.AddComponent<BoxCollider>();
            boxes[1] = con.AddComponent<BoxCollider>();
            boxes[2] = con.AddComponent<BoxCollider>();
            boxes[3] = con.AddComponent<BoxCollider>();

            ExpandColliders(boxes, positions);
        }

        void ExpandColliders(BoxCollider[] boxes, Vector3[] positions)
        {
            Vector2 halfSize;

            switch(_direction)
            {
                case EAnchorDirection.Forward:
                    CalibrateCollidersZ(boxes, positions);

                    halfSize = ExpandHalfSizeZ(boxes, positions[0], positions[3], (int)EColliderOrientation.Z);
                    boxes[2].size = new Vector3(halfSize[0] * 2f, 2f, halfSize[1] * 2f);
                    
                    break;

                case EAnchorDirection.Backward:
                    CalibrateCollidersZ(boxes, positions);

                    halfSize = ExpandHalfSizeZ(boxes, positions[0], positions[3], (int)EColliderOrientation.Z);
                    boxes[2].size = new Vector3(halfSize[0] * 2f, 2f, halfSize[1] * 2f);

                    break;

                case EAnchorDirection.Right:
                    CalibrateCollidersX(boxes, positions);
                    
                    halfSize = ExpandHalfSizeX(boxes, positions[0], positions[3], (int)EColliderOrientation.X);
                    boxes[2].size = new Vector3(halfSize[1] * 2f, 2f, halfSize[0] * 2f);
                    break;

                case EAnchorDirection.Left:
                    CalibrateCollidersX(boxes, positions);

                    halfSize = ExpandHalfSizeX(boxes, positions[0], positions[3], (int)EColliderOrientation.X);
                    boxes[2].size = new Vector3(halfSize[1] * 2f, 2f, halfSize[0] * 2f);
                    break;                
            }

            boxes[3].size = boxes[2].size;
        }

        void CalibrateCollidersZ(BoxCollider[] boxes, Vector3[] positions)
        {
            //Colliders exteriores
            boxes[0].center += new Vector3(_map.ConnectionCollidersOffset * Mathf.Abs(positions[0].x - positions[3].x) * 2f, 0f, 0f);
            boxes[1].center -= new Vector3(_map.ConnectionCollidersOffset * Mathf.Abs(positions[0].x - positions[3].x) * 2f, 0f, 0f);
            boxes[0].size = new Vector3(boxes[0].size.x, 2f, boxes[0].size.z * 0.8f);
            boxes[1].size = new Vector3(boxes[1].size.x, 2f, boxes[1].size.z * 0.8f);


            //Colliders interiores
            bool dirCond = (int)_direction % 2 == 0; //Si es 0, es forward o right
            Vector3 offset = new Vector3(0f, 0f, Mathf.Abs(positions[0].z - positions[3].z) / 4);

            boxes[2].center += (dirCond ? -1 : 1) * offset;
            boxes[3].center += (!dirCond ? -1 : 1) * offset;
            boxes[2].center = new Vector3(positions[3].x, boxes[2].center.y, boxes[2].center.z);
            boxes[3].center = new Vector3(positions[0].x, boxes[3].center.y, boxes[3].center.z);
            boxes[2].size = new Vector3(0f, 2f, 0f);
        }
        Vector2 ExpandHalfSizeZ(BoxCollider[] boxes, Vector3 start, Vector3 end, int axis)
        {
            Vector2 halfSize = new Vector2();
            float extraLimit = Mathf.Abs(start.x - end.x) * 0.08f;

            //Expansion hasta la mitad entre los puntos y un pequeno extra para que se adecue mejor a la mesh
            if (start[axis] < end[axis]) //comienza a la izquierda y termina a la derecha
            {
                while (boxes[2].center[axis] - halfSize[0] > (start[axis] + end[axis]) / 2 - extraLimit)
                {
                    halfSize[0] += 0.01f;
                }
            }
            else //comienza a la derecha y termina a la izquierda
            {
                while (boxes[2].center[axis] + halfSize[0] < (start[axis] + end[axis]) / 2 + extraLimit)
                {
                    halfSize[0] += 0.01f;
                }
            }

            //Expansion hasta la mitad de la conexion
            if (boxes[2].center[axis == 0 ? 2 : 0] < (start.z + end[axis == 0 ? 2 : 0]) / 2) //Si va hacia delante
            {
                while (boxes[2].center[axis == 0 ? 2 : 0] + halfSize[1] < (start.z + end[axis == 0 ? 2 : 0]) / 2 - Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10)
                {
                    halfSize[1] += 0.01f;
                }
            }
            else //Si va hacia atras
            {
                while (boxes[2].center[axis == 0 ? 2 : 0] - halfSize[1] > (start.z + end[axis == 0 ? 2 : 0]) / 2 + Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10)
                {
                    halfSize[1] += 0.01f;
                }
            }

            return halfSize;
        }

        void CalibrateCollidersX(BoxCollider[] boxes, Vector3[] positions)
        {
            //Colliders exteriores
            boxes[0].center += new Vector3(0f, 0f, _map.ConnectionCollidersOffset * Mathf.Abs(positions[0].z - positions[3].z) * 2f);
            boxes[1].center -= new Vector3(0f, 0f, _map.ConnectionCollidersOffset * Mathf.Abs(positions[0].z - positions[3].z) * 2f);
            boxes[0].size = new Vector3(boxes[0].size.x * 0.8f, 2f, boxes[0].size.z);
            boxes[1].size = new Vector3(boxes[1].size.x * 0.8f, 2f, boxes[1].size.z);


            //Colliders interiores
            bool dirCond = (int)_direction % 2 == 0; //Si es 0, es forward o right
            Vector3 offset = new Vector3(Mathf.Abs(positions[0].x - positions[3].x) / 4, 0f, 0f);

            boxes[2].center += (dirCond ? -1 : 1) * offset;
            boxes[3].center += (!dirCond ? -1 : 1) * offset;
            boxes[2].center = new Vector3(boxes[2].center.x, boxes[2].center.y, positions[3].z);
            boxes[3].center = new Vector3(boxes[3].center.x, boxes[3].center.y, positions[0].z);
            boxes[2].size = new Vector3(0f, 2f, 0f);
        }
        Vector2 ExpandHalfSizeX(BoxCollider[] boxes, Vector3 start, Vector3 end, int axis)
        {
            Vector2 halfSize = new Vector2();
            float extraLimit = Mathf.Abs(start.z - end.z) * 0.08f;

            //Expansion hasta la mitad entre los puntos y un pequeno extra para que se adecue mejor a la mesh
            if (start[axis] < end[axis]) //comienza a la izquierda y termina a la derecha
            {
                while (boxes[2].center[axis] - halfSize[0] > (start[axis] + end[axis]) / 2 - extraLimit)
                {
                    halfSize[0] += 0.01f;
                }
            }
            else //comienza a la derecha y termina a la izquierda
            {
                while (boxes[2].center[axis] + halfSize[0] < (start[axis] + end[axis]) / 2 + extraLimit)
                {
                    halfSize[0] += 0.01f;
                }
            }

            //Expansion hasta la mitad de la conexion
            Debug.Log($"Centro: {boxes[2].center[axis == 0 ? 2 : 0]} < Punto medio: {(start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2}; Despl: {Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10}");
            if (boxes[2].center[axis == 0 ? 2 : 0] < (start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2) //Si va hacia delante
            {
                while (boxes[2].center[axis == 0 ? 2 : 0] + halfSize[1] < (start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2 - Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10)
                {
                    
                    halfSize[1] += 0.01f;
                }
            }
            else //Si va hacia atras
            {
                Debug.Log($"Comprobacion: {boxes[2].center[axis == 0 ? 2 : 0] - halfSize[1]} > {(start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2 + Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10}");
                while (boxes[2].center[axis == 0 ? 2 : 0] - halfSize[1] > (start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2 + Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10)
                {
                    Debug.Log(halfSize);
                    halfSize[1] += 0.01f;
                }
            }

            return halfSize;
        }

        #endregion
    }
}