using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klijent
{
    public class GDAProxy : IDisposable
    {

        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        private static NetworkModelGDAProxy gdaQueryProxy = null;

        public NetworkModelGDAProxy GDAQueryProxy
        {
            get
            {
                return gdaQueryProxy;
            }

        }

        public GDAProxy()
        {
            if (gdaQueryProxy == null)
            {
                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                try
                {
                    gdaQueryProxy.Open();
                }
                catch
                {

                }
            }

        }


        public string GetValues(long globalId, List<ModelCode> props)
        {
            ResourceDescription rd = null;
            string ss = "";
            List<ModelCode> properties = new List<ModelCode>();
            try
            {
                short type = ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
                properties = props;
                // modelResourcesDesc.GetAllPropertyIds((DMSType)type);

                rd = GDAQueryProxy.GetValues(globalId, properties);
                ss += String.Format("Item with gid: 0x{0:x16}:\n", globalId);
                foreach (Property p in rd.Properties)
                {
                    ss += String.Format("\t{0} =", p.Id);
                    string retVal = GetStringForRichTB(p);
                    ss += retVal;
                    
                }
            }
            catch (Exception)
            {

            }

            return ss;
        }

        public string GetExtentValues(DMSType type, List<ModelCode> props)
        {

            int iteratorId = 0;
            List<long> ids = new List<long>();
            string ss = "";
            bool gidBool = true;
            ModelCode modelCode = modelResourcesDesc.GetModelCodeFromType(type);
            try
            {
                int numberOfResources = 2;
                int resourcesLeft = 0;

                List<ModelCode> properties = props;
                if (props.Contains(ModelCode.IDOBJ_GID) == false)
                {
                    properties.Add(ModelCode.IDOBJ_GID);
                    gidBool = false;
                }
                iteratorId = GDAQueryProxy.GetExtentValues(modelCode, properties);
                resourcesLeft = GDAQueryProxy.IteratorResourcesLeft(iteratorId);
                ss += String.Format("Items with ModelCode: {0}:\n", modelCode.ToString());
                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GDAQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        ss += String.Format("\tItem with gid: 0x{0:x16}\n", rds[i].Properties.Find(r => r.Id == ModelCode.IDOBJ_GID).AsLong());
                        foreach (Property p in rds[i].Properties)
                        {
                            if (p.Id == ModelCode.IDOBJ_GID && gidBool == false)
                            {

                            }
                            else
                            {
                                ss += String.Format("\t\t{0} =", p.Id);
                                string retVal = GetStringForRichTB(p);
                                ss += retVal;
                            }
                        }
                    }
                    resourcesLeft = GDAQueryProxy.IteratorResourcesLeft(iteratorId);
                }

                GDAQueryProxy.IteratorClose(iteratorId);

            }
            catch (Exception)
            {

            }


            return ss;
        }

        public string GetRelatedValues(long sourceGlobalId, Association association, List<ModelCode> props)
        {

            string ss = "";
            int numberOfResources = 2;
            bool gidBool = true;
            try
            {
                List<ModelCode> properties = props;
                if (props.Contains(ModelCode.IDOBJ_GID) == false)
                {
                    properties.Add(ModelCode.IDOBJ_GID);
                    gidBool = false;
                }
                int iteratorId = GDAQueryProxy.GetRelatedValues(sourceGlobalId, properties, association);
                int resourcesLeft = GDAQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    try
                    {
                        List<ResourceDescription> rds = GDAQueryProxy.IteratorNext(numberOfResources, iteratorId);

                        for (int i = 0; i < rds.Count; i++)
                        {
                            ss += String.Format("Item with gid: 0x{0:x16}\n", rds[i].Properties.Find(r => r.Id == ModelCode.IDOBJ_GID).AsLong());
                            foreach (Property p in rds[i].Properties)
                            {
                                if (p.Id == ModelCode.IDOBJ_GID && gidBool == false)
                                {

                                }
                                else
                                {
                                    ss += String.Format("\t{0} =", p.Id);
                                    string retVal = GetStringForRichTB(p);
                                    ss += retVal;

                                }
                            }
                        }
                        resourcesLeft = GDAQueryProxy.IteratorResourcesLeft(iteratorId);
                    }
                    catch(Exception)
                    {

                    }
                    
                }

                GDAQueryProxy.IteratorClose(iteratorId);


            }
            catch (Exception)
            {

            }

            return ss;
        }

        public List<long> GetAllGids()
        {
            ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
            List<ModelCode> properties = new List<ModelCode>();
            List<long> ids = new List<long>();

            int iteratorId = 0;
            int numberOfResources = 1000;
            DMSType currType = 0;
            properties.Add(ModelCode.IDOBJ_GID);
            try
            {
                foreach (DMSType type in Enum.GetValues(typeof(DMSType)))
                {
                    currType = type;

                    if (type != DMSType.MASK_TYPE)
                    {
                        iteratorId = GDAQueryProxy.GetExtentValues(modelResourcesDesc.GetModelCodeFromType(type), properties);
                        int count = GDAQueryProxy.IteratorResourcesLeft(iteratorId);

                        while (count > 0)
                        {
                            List<ResourceDescription> rds = GDAQueryProxy.IteratorNext(numberOfResources, iteratorId);

                            for (int i = 0; i < rds.Count; i++)
                            {
                                ids.Add(rds[i].Id);
                            }

                            count = GDAQueryProxy.IteratorResourcesLeft(iteratorId);
                        }

                        bool ok = GDAQueryProxy.IteratorClose(iteratorId);

                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ids;
        }

        public string GetStringForRichTB(Property p)
        {
            string ss = "";
            switch (p.Type)
            {
                case PropertyType.Float:
                    ss += String.Format(" {0}\n", p.AsFloat());
                    break;
                case PropertyType.DateTime:
                    ss += String.Format("{0}\n", p.AsDateTime());
                    break;
                case PropertyType.Bool:
                    ss += String.Format("{0}\n", (p.AsInt() == 1) ? true : false);
                    break;
                case PropertyType.Int32:
                case PropertyType.Int64:
                    if (p.Id == ModelCode.IDOBJ_GID)
                    {
                        ss += (String.Format("0x{0:x16}\n", p.AsLong()));
                    }
                    else
                    {
                        ss += String.Format("{0}\n", p.AsLong());
                    }
                    break;
                case PropertyType.Reference:
                    ss += (String.Format("0x{0:x16}\n", p.AsReference()));
                    break;
                case PropertyType.String:
                    if (p.PropertyValue.StringValue == null)
                    {
                        p.PropertyValue.StringValue = String.Empty;
                    }
                    ss += String.Format("{0}\n", p.AsString());
                    break;
                case PropertyType.ReferenceVector:
                    if (p.AsLongs().Count > 0)
                    {
                        string s = "";
                        for (int j = 0; j < p.AsLongs().Count; j++)
                        {
                            s += (String.Format("0x{0:x16},\n", p.AsLongs()[j]));
                            
                        }
                        ss += s;
                    }
                    else
                    {
                        ss += ("empty long/reference vector\n");
                    }
                    break;
                case PropertyType.Enum:

                    List<string> listEnums = new EnumDescs().GetEnumList(p.Id);
                    ss += (String.Format(listEnums[Int32.Parse(p.ToString())]) + "\n");
                    break;


                default:
                    throw new Exception("Failed to export Resource Description as XML. Invalid property type.");
            }
            return ss;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
