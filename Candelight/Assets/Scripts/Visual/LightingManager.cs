using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Visual
{
    public class LightingManager : MonoBehaviour
    {
        public NodeInfo CurrentNodeInfo;
        [SerializeField] MeshRenderer _ground;

        [Header("Durnia Settings")]
        [SerializeField] Color _dFog;
        [SerializeField] float _dFogDistance;
        [Space(7)]
        [SerializeField] Material _dGround;
        [SerializeField] Material _dConnection;

        [Space(20)]
        [Header("Temeria Settings")]
        [SerializeField] Color _tFog;
        [SerializeField] float _tFogDistance;
        [Space(7)]
        [SerializeField] Material _tGround;
        [SerializeField] Material _tConnection;

        [Space(20)]
        [Header("Idria Settings")]
        [SerializeField] Color _iFog;
        [SerializeField] float _iFogDistance;
        [Space(7)]
        [SerializeField] Material _iGround;
        [SerializeField] Material _iConnection;


        private void Start()
        {
            LoadBiomeVisuals(CurrentNodeInfo.Biome);
        }

        public void LoadBiomeVisuals(EBiome biome)
        {
            switch(biome)
            {
                case EBiome.Durnia:
                    RenderSettings.fogColor = _dFog;
                    RenderSettings.fogEndDistance = _dFogDistance;

                    _ground.material = _dGround;

                    break;
                case EBiome.Temeria:
                    RenderSettings.fogColor = _tFog;
                    RenderSettings.fogEndDistance = _tFogDistance;

                    _ground.material = _tGround;

                    break;
                case EBiome.Idria:
                    RenderSettings.fogColor = _iFog;
                    RenderSettings.fogEndDistance = _iFogDistance;

                    _ground.material = _iGround;

                    break;
                default:
                    Debug.LogWarning("ERROR: Bioma no detectado.");
                    break;
            }
        }

        public Material GetConnectionMaterial(EBiome biome)
        {
            switch(biome)
            {
                case EBiome.Durnia:
                    return _dConnection;
                case EBiome.Temeria:
                    return _tConnection;
                case EBiome.Idria:
                    return _iConnection;
                default:
                    return null;
            }
        }
    }
}