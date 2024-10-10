using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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

        bool _active;
        public bool Connected;

        GameObject _connectionMesh;

        private void Awake()
        {
            switch (_direction)
            {
                case EAnchorDirection.Forward:
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case EAnchorDirection.Backward:
                    transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
                case EAnchorDirection.Left:
                    transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                    break;
                case EAnchorDirection.Right:
                    transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    break;
               
            }
            _raycastDirection = transform.forward;
            _auxDirLeft = transform.forward - transform.right;
            _auxDirLeft.Normalize();
            _auxDirRight = transform.forward + transform.right;
            _auxDirRight.Normalize();

            _room = GetComponentInParent<ARoom>();
        }

        private void Start()
        {
            _map = MapManager.Instance;

            _active = Random.value < 0.5f;
            if (_active && _map.CanCreateRoom()) SpawnRoom();
            else StartCoroutine(CreateNewRoom());
        }

        private void Update()
        {
            //if (_active)
            //{
            //    Debug.DrawRay(transform.position, _raycastDirection * _map.MediumThreshold, Color.red);
            //    Debug.DrawRay(transform.position, _auxDirLeft * _map.MediumThreshold, Color.yellow);
            //    Debug.DrawRay(transform.position, _auxDirRight * _map.MediumThreshold, Color.yellow);
            //    Debug.DrawRay(transform.position + (_map.MediumThreshold * transform.forward - _map.SmallThreshold * transform.right), _map.SmallThreshold * transform.right, Color.blue);
            //}
        }

        IEnumerator CreateNewRoom()
        {
            yield return new WaitForSeconds(0.1f);
            if (_map.CanCreateRoom()) SpawnRoom();
            else Destroy(gameObject);
        }


        /// <summary>
        /// Generamos una nueva habitacion a raiz de un anclaje, si se puede
        /// </summary>
        /// <returns></returns>
        void SpawnRoom()
        {
            //Averiguamos el tamano maximo 
            int maxSize = Physics.Raycast(transform.position, _raycastDirection, out _hit, _map.MediumThreshold) ? _hit.distance < _map.SmallThreshold ? _hit.distance < _map.RoomSeparation ? -1 : 0 : 1 : 2;
            if (Physics.Raycast(transform.position, _auxDirLeft, out _auxLhit, _map.MediumThreshold) || Physics.Raycast(transform.position, _auxDirRight, out _auxRhit, _map.MediumThreshold)) maxSize = 0;
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

        GameObject GenerateMesh(AnchorManager obj)
        {
            GameObject lineGO = new GameObject("Transicion");
            lineGO.transform.parent = transform;
            LineRenderer line = lineGO.AddComponent<LineRenderer>();

            //Se configura la linea que une ambos anclajes para conseguir la forma deseada
            line.positionCount = 4;
            line.widthMultiplier = 0.5f;
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

            //Se bakea la imagen de la linea en una mesh
            Mesh transitionMesh = new Mesh();
            line.BakeMesh(transitionMesh);
            GameObject meshGO = new GameObject("TransitionMesh");
            meshGO.transform.parent = transform;
            MeshRenderer meshData = meshGO.AddComponent<MeshRenderer>();
            MeshFilter mesh = meshGO.AddComponent<MeshFilter>();
            mesh.mesh = transitionMesh;

            line.enabled = false;

            if (Mathf.Abs(positions[0].x - positions[3].x) < 0.1f) GenerateSimpleColliders(meshGO, positions);
            else GenerateComplexColliders(meshGO, positions, line.endWidth);

            return meshGO;
        }

        void GenerateSimpleColliders(GameObject con, Vector3[] positions)
        {
            BoxCollider box1 = con.AddComponent<BoxCollider>();
            BoxCollider box2 = con.AddComponent<BoxCollider>();
            box1.center += _map.ConnectionCollidersOffset * transform.right;
            box2.center -= _map.ConnectionCollidersOffset * transform.right;
            //if (_direction == EAnchorDirection.Forward || _direction == EAnchorDirection.Backward)
            //{
            //    box1.size = new Vector3(box1.size.x, 2f, box1.size.z * 0.9f);
            //    box2.size = new Vector3(box1.size.x, 2f, box1.size.z * 0.9f);
            //}
            //else
            //{
            //    box1.size = new Vector3(box1.size.x * 0.9f, 2f, box1.size.z);
            //    box2.size = new Vector3(box1.size.x * 0.9f, 2f, box1.size.z);
            //}
            box1.size = new Vector3(box1.size.x, 2f, box1.size.z * 0.8f);
            box2.size = new Vector3(box1.size.x, 2f, box1.size.z * 0.8f);
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

        //Vector2 ExpandHalfSize(BoxCollider[] boxes, Vector3 start, Vector3 end)
        //{
        //    float halfSizeX = 0f;
        //    float extraLimit = Mathf.Abs(start.x - end.x) * 0.1f;
        //    float halfSizeZ = 0f;
        //
        //    //Expansion hasta la mitad entre los puntos y un pequeno extra para que se adecue mejor a la mesh
        //    if (start.x < end.x) //comienza a la izquierda y termina a la derecha
        //    {
        //        while (boxes[2].center.x - halfSizeX > (start.x + end.x) / 2 - extraLimit)
        //        {
        //            halfSizeX += 0.01f;
        //        }
        //    }
        //    else //comienza a la derecha y termina a la izquierda
        //    {
        //        while (boxes[2].center.x + halfSizeX < (start.x + end.x) / 2 + extraLimit)
        //        {
        //            halfSizeX += 0.01f;
        //        }
        //    }
        //    //Expansion hasta la mitad de la conexion
        //    while (boxes[2].center.z + halfSizeZ < (start.z + end.z) / 2 - Mathf.Abs(start.z - end.z) / 10)
        //    {
        //        halfSizeZ += 0.01f;
        //    }
        //
        //    return new Vector2(halfSizeX, halfSizeZ);
        //}

        void CalibrateCollidersZ(BoxCollider[] boxes, Vector3[] positions)
        {
            //Colliders exteriores
            boxes[0].center += new Vector3(_map.ConnectionCollidersOffset * Mathf.Abs(positions[0].x - positions[3].x) * 3.8f, 0f, 0f);
            boxes[1].center -= new Vector3(_map.ConnectionCollidersOffset * Mathf.Abs(positions[0].x - positions[3].x) * 3.8f, 0f, 0f);
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
            boxes[0].center += new Vector3(0f, 0f, _map.ConnectionCollidersOffset * Mathf.Abs(positions[0].z - positions[3].z) * 3.8f);
            boxes[1].center -= new Vector3(0f, 0f, _map.ConnectionCollidersOffset * Mathf.Abs(positions[0].z - positions[3].z) * 3.8f);
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
    }
}