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

        [SerializeField] GameObject _sprite;

        LayerMask _mask;

        bool _active;
        public bool Connected;
        public AnchorManager ConnectedAnchor;

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

        public ARoom GetRoom() => _room;

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

            _sprite.GetComponent<SpriteRenderer>().sharedMaterial.SetInt("_Highlight", 0);
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
            Debug.DrawRay(transform.position, _map.MediumThreshold * transform.forward, Color.blue);
            //Debug.DrawRay(transform.position + _map.MediumThreshold * transform.forward, transform.right * _map.SmallThreshold, Color.red);
            //Debug.DrawRay(transform.position + _map.MediumThreshold * transform.forward, );
        }

        /// <summary>
        /// Se aplica un delay antes de que cree la habitacion. Si no puede crearla, se autodestruye
        /// </summary>
        /// <returns></returns>
        IEnumerator DelayedRoomCreation()
        {
            yield return new WaitForSeconds(0.25f);
            if (_map.CanCreateRoom()) SpawnRoom();
            //else Destroy(gameObject);
        }


        /// <summary>
        /// Generamos una nueva habitacion a raiz de un anclaje si el espacio lo permite
        /// </summary>
        /// <returns></returns>
        void SpawnRoom()
        {
            //Averiguamos el tamano maximo 
            int maxSize = Physics.Raycast(transform.position + MapManager.Instance.AnchorCastOrigin * transform.forward, _raycastDirection, out _hit, _map.MediumThreshold, ~_mask) ? _hit.distance < _map.SmallThreshold ? _hit.distance < _map.RoomSeparation ? -1 : 0 : 1 : 2;
            //Debug.Log($"Tamano original: {maxSize} (Distancia libre: {_hit.distance})");
            //if (_hit.transform != null) Debug.Log($"Soy {gameObject.name} ({transform.parent.gameObject.name}) y he colisionado con {_hit.transform.gameObject.name} ({_hit.transform.parent.gameObject.name})");
            
            if (Physics.Raycast(transform.position + MapManager.Instance.AnchorCastOrigin * transform.forward, _auxDirLeft, out _auxLhit, _map.MediumThreshold, ~_mask) && _auxLhit.transform.parent.parent != transform.parent ||
                Physics.Raycast(transform.position + MapManager.Instance.AnchorCastOrigin * transform.forward, _auxDirRight, out _auxRhit, _map.MediumThreshold, ~_mask) && _auxRhit.transform.parent.parent != transform.parent) maxSize -= 1;
            //Debug.Log($"Colisiona con {_hit.transform.name}");
            Debug.Log("Prev MAX_SIZE: " + maxSize);
            if (Physics.CheckSphere(transform.position + _map.MediumThreshold * transform.forward, _map.SmallThreshold * 0.75f)) maxSize = -1;
            Debug.Log(Physics.CheckSphere(transform.position + _map.MediumThreshold * transform.forward, _map.SmallThreshold * 0.75f));
            if (maxSize == -1) return;

            ERoomSize size = (ERoomSize)Random.Range(0, maxSize + 1);

            //Generamos un tamano segun el maximo calculado y la posicion que le corresponde
            Vector3 localOffset = OffsetChooser(size);

            //Procesamos cual es la posicion que le corresponde en el minimapa
            Vector2 minimapOffset = MinimapOffsetChooser();

            //Generamos la nueva sala y encontramos su anclaje
            Debug.Log("GENERO NUEVA HABITACION");
            GameObject newRoom = _map.RegisterNewRoom(_room.GetID(), transform.position + localOffset, minimapOffset, size);
            ConnectedAnchor = LocateObjectiveAnchor(newRoom);
            _map.NotifyNewRoom(newRoom, minimapOffset);

            //Suponiendo que existe el anclaje (en caso contrario, llevaria una referencia a si mismo para evitar errores), generamos la malla para la transicion entre ambos
            _connectionMesh = GenerateMesh(ConnectedAnchor);
            SetUVToWorld uvScript = _connectionMesh.AddComponent<SetUVToWorld>();
            uvScript.MaterialToUse = _map.ConnectionMaterial;
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

        Vector2 MinimapOffsetChooser()
        {
            Vector2 miniOffset = _room.GetMinimapOffset();
            switch (_direction)
            {
                case EAnchorDirection.Forward:
                    miniOffset += new Vector2(0f, 40f);
                    break;
                case EAnchorDirection.Backward:
                    miniOffset += new Vector2(0f, -40f);
                    break;
                case EAnchorDirection.Right:
                    miniOffset += new Vector2(40f, 0f);
                    break;
                case EAnchorDirection.Left:
                    miniOffset += new Vector2(-40f, 0f);
                    break;
                default:
                    break;
            }
            return miniOffset;
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
            //float minDist = 9999f;
            //AnchorManager obj = this; //Pongo this en vez de null para que no de problemas en caso de fallo horrible dios no lo quiera
            //foreach (var a in anchors)
            //{
            //    float dist = Vector3.Distance(transform.position, a.transform.position);
            //    if (dist < minDist)
            //    {
            //        minDist = dist;
            //        obj = a;
            //    }
            //}

            switch(_direction)
            {
                case EAnchorDirection.Forward:
                    foreach (var a in anchors)
                    {
                        if (a.GetDirection() == EAnchorDirection.Backward) return a;
                    }
                    break;
                case EAnchorDirection.Backward:
                    foreach (var a in anchors)
                    {
                        if (a.GetDirection() == EAnchorDirection.Forward) return a;
                    }
                    break;
                case EAnchorDirection.Left:
                    foreach (var a in anchors)
                    {
                        if (a.GetDirection() == EAnchorDirection.Right) return a;
                    }
                    break;
                case EAnchorDirection.Right:
                    foreach (var a in anchors)
                    {
                        if (a.GetDirection() == EAnchorDirection.Left) return a;
                    }
                    break;
            }

            return this;
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

            //Destroy(obj.gameObject);
            obj.OpenAnchor();
            //obj.GetComponent<Collider>().enabled = false;
            
            //GetComponent<Collider>().enabled = false;
            OpenAnchor();
            Connected = true;

            //Se bakea la imagen de la linea en una mesh
            Mesh transitionMesh = new Mesh();
            line.BakeMesh(transitionMesh, _map.ConnectionBakeCam);
            GameObject meshGO = new GameObject("TransitionMesh");
            meshGO.transform.parent = transform;
            meshGO.transform.localPosition = new Vector3(meshGO.transform.localPosition.x, 0.009f, meshGO.transform.localPosition.z);
            MeshRenderer meshData = meshGO.AddComponent<MeshRenderer>();
            MeshFilter mesh = meshGO.AddComponent<MeshFilter>();
            mesh.mesh = transitionMesh;

            //Recalculamos las normales para que la iluminacion funcione bien
            mesh.mesh.RecalculateNormals();

            line.enabled = false;

            //Colliders
            BoxCollider baseCol = meshGO.AddComponent<BoxCollider>();
            baseCol.center = new Vector3(baseCol.center.x, 0f, baseCol.center.z);

            //Debug.Log($"({_room.GetID()}) Diferencia en X: {positions[0].x - positions[3].x}");
            if (Mathf.Abs(positions[0].x - positions[3].x) < 1.4f && Mathf.Abs(positions[0].z - positions[3].z) > 1f) GenerateSimpleCollidersZ(meshGO, positions);
            else if (Mathf.Abs(positions[0].z - positions[3].z) < 1.4f && Mathf.Abs(positions[0].x - positions[3].x) > 1f) GenerateSimpleCollidersX(meshGO, positions);
            else GenerateComplexColliders(meshGO, positions, line.endWidth);

            return meshGO;
        }

        #endregion

        //TERCERA FASE:
        // Segun las propiedades de la conexion, se decide si generar colliders simples o complejos
        // Se crean BoxColliders. Se altera su center y size para que se adecuen a su correspondiente conexion
        #region Third Stage: Collider Generation

        /// <summary>
        /// Si la conexion es vertical, se generan dos colliders a ambos lados del camino
        /// </summary>
        /// <param name="con"></param>
        /// <param name="positions"></param>
        void GenerateSimpleCollidersZ(GameObject con, Vector3[] positions)
        {
            float zReduction = 0.8f;

            BoxCollider box1 = con.AddComponent<BoxCollider>();
            BoxCollider box2 = con.AddComponent<BoxCollider>();

            //Se colocan acorde a un offset
            box1.center += _map.ConnectionCollidersOffset * 2f * Vector3.right;
            box2.center -= _map.ConnectionCollidersOffset * 2f * Vector3.right;
            //Se expanden una cantidad que se ha considerado correcta
            box1.size = new Vector3(box1.size.x, 2f, box1.size.z * zReduction);
            box2.size = new Vector3(box1.size.x, 2f, box1.size.z * zReduction);

            float deltaCenters = Mathf.Abs(box1.center.x - box2.center.x);
            float offset = 0;
            //Debug.Log($"Soy {_room.GetID()} con distancia en X: {deltaCenters}");
            if (deltaCenters > 5f) offset = 1.7f;

            box1.center -= offset * Vector3.right;
            box2.center += offset * Vector3.right;
        }

        /// <summary>
        /// Si la conexion es horizontal, se generan dos colliders a ambos lados del camino
        /// </summary>
        /// <param name="con"></param>
        /// <param name="positions"></param>
        void GenerateSimpleCollidersX(GameObject con, Vector3[] positions)
        {
            float xReduction = 0.8f;

            BoxCollider box1 = con.AddComponent<BoxCollider>();
            BoxCollider box2 = con.AddComponent<BoxCollider>();
            //Se colocan acorde a un offset
            box1.center += _map.ConnectionCollidersOffset * 2f * Vector3.forward;
            box2.center -= _map.ConnectionCollidersOffset * 2f * Vector3.forward;
            //Se expanden una cantidad que se ha considerado correcta
            box1.size = new Vector3(box1.size.x * xReduction, 2f, box1.size.z);
            box2.size = new Vector3(box1.size.x * xReduction, 2f, box1.size.z);

            float deltaCenters = Mathf.Abs(box1.center.x - box2.center.x);
            float offset = 0;
            //Debug.Log($"Soy {_room.GetID()} con distancia en X: {deltaCenters}");
            if (deltaCenters > 5f) offset = 1.55f;

            box1.center -= offset * Vector3.right;
            box2.center += offset * Vector3.right;
        }

        //Generacion de colliders complejos
        void GenerateComplexColliders(GameObject con, Vector3[] positions, float lineWidth)
        {
            BoxCollider[] boxes = new BoxCollider[4];

            boxes[0] = con.AddComponent<BoxCollider>();
            boxes[1] = con.AddComponent<BoxCollider>();
            boxes[2] = con.AddComponent<BoxCollider>();
            boxes[3] = con.AddComponent<BoxCollider>();

            ExpandColliders(boxes, positions);
        }

        /// <summary>
        /// Se expanden los colliders en tamano. Cada anchor tiene una direccion y gracias a ella podemos predecir que direccion tendra la conexion (horizontal o vertical).
        /// 
        /// </summary>
        /// <param name="boxes"></param>
        /// <param name="positions"></param>
        void ExpandColliders(BoxCollider[] boxes, Vector3[] positions)
        {
            Vector2 halfSize;

            float anchorDeltaX;
            float anchorDeltaZ;
            float extraSize;

            switch (_direction)
            {
                case EAnchorDirection.Forward:
                    CalibrateCollidersZ(boxes, positions);

                    halfSize = ExpandHalfSizeZ(boxes, positions[0], positions[3], (int)EColliderOrientation.Z);

                    anchorDeltaX = Mathf.Abs(positions[0].x - positions[3].x);
                    if (anchorDeltaX > 4f) extraSize = 0.4f * anchorDeltaX; //Valor exacto en >4f encontrado para 7.47
                    else if (anchorDeltaX > 2f) extraSize = 0.7f * anchorDeltaX;
                    else extraSize = 0.9f * anchorDeltaX;

                    boxes[2].size = new Vector3(halfSize[0] * extraSize, 2f, halfSize[1] * 2.1f);
                    
                    break;

                case EAnchorDirection.Backward:
                    CalibrateCollidersZ(boxes, positions);

                    halfSize = ExpandHalfSizeZ(boxes, positions[0], positions[3], (int)EColliderOrientation.Z);

                    anchorDeltaX = Mathf.Abs(positions[0].x - positions[3].x);
                    if (anchorDeltaX > 4f) extraSize = 0.4f * anchorDeltaX; //Valor exacto en >4f encontrado para 7.47
                    else if (anchorDeltaX > 2f) extraSize = 0.7f * anchorDeltaX;
                    else extraSize = 0.9f * anchorDeltaX;

                    boxes[2].size = new Vector3(halfSize[0] * extraSize, 2f, halfSize[1] * 2.1f);

                    break;

                case EAnchorDirection.Right:
                    CalibrateCollidersX(boxes, positions);

                    anchorDeltaZ = Mathf.Abs(positions[0].z - positions[3].z);
                    //Debug.Log($"Soy {_room.GetID()} con distancia en Z: {anchorDeltaZ}");
                    if (anchorDeltaZ > 4f) extraSize = 0.4f * anchorDeltaZ; //Valor exacto en >4f encontrado para 7.47
                    else extraSize = 0.9f * anchorDeltaZ;

                    halfSize = ExpandHalfSizeX(boxes, positions[0], positions[3], (int)EColliderOrientation.X);
                    boxes[2].size = new Vector3(halfSize[1] * 2.1f, 2f, halfSize[0] * extraSize);
                    break;

                case EAnchorDirection.Left:
                    CalibrateCollidersX(boxes, positions);

                    anchorDeltaZ = Mathf.Abs(positions[0].z - positions[3].z);
                    //Debug.Log($"Soy {_room.GetID()} con distancia en Z: {anchorDeltaZ}");
                    if (anchorDeltaZ > 4f) extraSize = 0.4f * anchorDeltaZ; //Valor exacto en >4f encontrado para 7.47
                    else extraSize = 0.9f * anchorDeltaZ;

                    halfSize = ExpandHalfSizeX(boxes, positions[0], positions[3], (int)EColliderOrientation.X);
                    boxes[2].size = new Vector3(halfSize[1] * 2.1f, 2f, halfSize[0] * extraSize);
                    break;                
            }

            boxes[3].size = boxes[2].size;
        }

        void CalibrateCollidersZ(BoxCollider[] boxes, Vector3[] positions)
        {
            //=== COLLIDERS EXTERIORES ===

            //La distancia en el eje X entre ambos anchors.
            float anchorDeltaX = Mathf.Abs(positions[0].x - positions[3].x);
            //Debug.Log($"Soy {_room.GetID()} con distancia en X: {anchorDeltaX}");

            //Se elije segun la distancia.
            // Para deltaX > 4: 1.27f (posicion exacta conseguida en 7.47)
            // Para deltaX > 3: 1.7f
            // Para deltaX <= 3: 1.85f (posicion exacta conseguida en 2.056)
            float extraDist = 0f;
            if (anchorDeltaX > 4f) extraDist = 1.27f;
            else if (anchorDeltaX > 3f) extraDist = 1.7f;
            else if (anchorDeltaX > 2f) extraDist = 1.85f;
            else extraDist = 2.2f;

            //Para elegir el centro de cada conexion, se toma el valor del offset y se multiplica por la distancia
            //Cuanto mas lejos esten los anchors en el eje X, mas se alejaran los colliders
            boxes[0].center += new Vector3(anchorDeltaX * extraDist, 0f, 0f);
            boxes[1].center -= new Vector3(anchorDeltaX * extraDist, 0f, 0f);
            boxes[0].size = new Vector3(boxes[0].size.x, 2f, boxes[0].size.z * 0.65f);
            boxes[1].size = new Vector3(boxes[1].size.x, 2f, boxes[1].size.z * 0.65f);


            //=== COLLIDERS INTERIORES ===

            bool dirCond = (int)_direction % 2 == 0; //Si es 0, es forward o right
            Vector3 offset = new Vector3(0f, 0f, Mathf.Abs(positions[0].z - positions[3].z) * 0.25f);

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
            //=== COLLIDERS EXTERIORES ===

            //La distancia en el eje X entre ambos anchors.
            float anchorDeltaZ = Mathf.Abs(positions[0].z - positions[3].z);
            //Debug.Log($"Soy {_room.GetID()} con distancia en Z: {anchorDeltaZ}");

            //La distancia extra se elije segun la distancia.
            // Para deltaX > 4: 1.27f (posicion exacta conseguida en 7.47)
            // Para deltaX > 3: 1.7f
            // Para deltaX <= 3: 1.85f (posicion exacta conseguida en 2.056)
            float extraDist = 0f;
            if (anchorDeltaZ > 4f) extraDist = 1.27f;
            else if (anchorDeltaZ > 3f) extraDist = 1.7f;
            else if (anchorDeltaZ > 2f) extraDist = 1.85f;
            else extraDist = 2.2f;

            boxes[0].center += new Vector3(0f, 0f, anchorDeltaZ * extraDist);
            boxes[1].center -= new Vector3(0f, 0f, anchorDeltaZ * extraDist);
            boxes[0].size = new Vector3(boxes[0].size.x * 0.8f, 2f, boxes[0].size.z);
            boxes[1].size = new Vector3(boxes[1].size.x * 0.8f, 2f, boxes[1].size.z);


            //=== COLLIDERS INTERIORES ===
          
            bool dirCond = (int)_direction % 2 == 0; //Si es 0, es forward o right
            Vector3 offset = new Vector3(Mathf.Abs(positions[0].x - positions[3].x) * 0.25f, 0f, 0f);

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
            //Debug.Log($"Centro: {boxes[2].center[axis == 0 ? 2 : 0]} < Punto medio: {(start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2}; Despl: {Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10}");
            if (boxes[2].center[axis == 0 ? 2 : 0] < (start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2) //Si va hacia delante
            {
                while (boxes[2].center[axis == 0 ? 2 : 0] + halfSize[1] < (start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2 - Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10)
                {
                    
                    halfSize[1] += 0.01f;
                }
            }
            else //Si va hacia atras
            {
                //Debug.Log($"Comprobacion: {boxes[2].center[axis == 0 ? 2 : 0] - halfSize[1]} > {(start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2 + Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10}");
                while (boxes[2].center[axis == 0 ? 2 : 0] - halfSize[1] > (start[axis == 0 ? 2 : 0] + end[axis == 0 ? 2 : 0]) / 2 + Mathf.Abs(start[axis == 0 ? 2 : 0] - end[axis == 0 ? 2 : 0]) / 10)
                {
                    halfSize[1] += 0.01f;
                }
            }

            return halfSize;
        }

        #endregion

        public void OpenAnchor()
        {
            GetComponent<Collider>().enabled = false;
            _sprite.SetActive(false);
            _sprite.GetComponent<SpriteRenderer>().sharedMaterial.SetInt("_Highlight", 0);

            if (!_room.AvailableAnchors.Contains(this)) _room.AvailableAnchors.Add(this);
        }

        public void CloseAnchor()
        {
            _sprite.GetComponent<SpriteRenderer>().sharedMaterial.SetInt("_Highlight", 1);
            GetComponent<Collider>().enabled = true;
            _sprite.SetActive(true);
        }

        public EAnchorDirection GetDirection() => _direction;
    }
}