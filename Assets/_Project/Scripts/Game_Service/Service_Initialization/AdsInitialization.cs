using System.Collections.Generic;
using UnityEngine;

namespace TheBeginning.Services
{
    public class AdsInitialization : ServiceInitialization
    {
        [SerializeField] private List<AdVariable> listAdVariable;

        public override void Initialization()
        {
            listAdVariable.ForEach(x => x.Init());
        }
    }
}