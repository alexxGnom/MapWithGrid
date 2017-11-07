using UnityEngine;
using System.Collections.Generic;

namespace EnixanTest
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField]
        private Transform _container;

        [Header("Prototypes:")]

        public List<GameObject> Units_Prototypes;

        public GameObject Indicator_Prototype;

        //Pools

        public List<UnityPool<BaseUnit>> Units_Pools { get; protected set; }

        public UnityPool<Indicator> Indicator_Pool { get; protected set; }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        protected override void Init()
        {
            base.Init();

            if (_container == null)
            {
                var obj = new GameObject("Pools");
                _container = obj.transform;
                _container.SetParent(transform, false);
            }

            InitPools();

            Precache();
        }

        protected void InitPools()
        {
            Units_Pools = new List<UnityPool<BaseUnit>>();
            for (var i = 0; i < Units_Prototypes.Count; i++)
            {
                int id = i;
                var prototype = Units_Prototypes[id].GetComponent<BaseUnit>();
                Units_Pools.Add(new UnityPool<BaseUnit>(() => Instantiate(prototype), _container, string.Format("{0}_{1}",prototype.Description, i)));
            }

            Indicator_Pool = new UnityPool<Indicator>(() => Instantiate(Indicator_Prototype).GetComponent<Indicator>(), _container, "Indicator");
        }

        protected void Precache()
        {
            for (int i = 0; i < Units_Pools.Count; i++)
            {
                Units_Pools[i].Cache(10);
            }

            Indicator_Pool.Cache(10);
        }
    }
}