using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klijent
{
    public static class PomocneMetode
    {
        public static List<ModelCode> GetProperties(long gid)
        {
            ModelResourcesDesc mr = new ModelResourcesDesc();
            List<ModelCode> list = mr.GetAllPropertyIdsForEntityId(gid);

            return list;
        }

        public static List<ModelCode> GetProperties2(DMSType code)
        {
            ModelResourcesDesc mr = new ModelResourcesDesc();
            List<ModelCode> list = mr.GetAllPropertyIds(code);

            return list;
        }

        
        public static List<ModelCode> GetRefIds(long gid3)
        {
            ModelResourcesDesc mr = new ModelResourcesDesc();
            List<ModelCode> list = mr.GetAllPropertyIdsForEntityId(gid3);
            List<ModelCode> ret = new List<ModelCode>();

            foreach (ModelCode m in list)
            {
                if (Property.GetPropertyType(m) == PropertyType.Reference ||
                    Property.GetPropertyType(m) == PropertyType.ReferenceVector)
                {
                    ret.Add(m);
                }

            }

            return ret;

        }
    }
}
