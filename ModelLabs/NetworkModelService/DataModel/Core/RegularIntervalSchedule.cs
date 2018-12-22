using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class RegularIntervalSchedule : BasicIntervalSchedule
    {
        private List<long> timePoints = new List<long>();

        public RegularIntervalSchedule(long globalId) : base(globalId)
        {
        }

        public List<long> TimePoints { get => timePoints; set => timePoints = value; }

        public override bool Equals(object obj)
        {
            RegularIntervalSchedule x = (RegularIntervalSchedule)obj;
            if (base.Equals(obj))
            {
                return (CompareHelper.CompareLists(x.timePoints, this.timePoints));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.REGULARINTERVALSCHEDULE_REGULARTIMEPOINTS:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.REGULARINTERVALSCHEDULE_REGULARTIMEPOINTS:
                    prop.SetValue(timePoints);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }

        }




        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return (timePoints.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (timePoints != null && timePoints.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULARINTERVALSCHEDULE_REGULARTIMEPOINTS] = timePoints.GetRange(0, timePoints.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE:
                    timePoints.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE:

                    if (timePoints.Contains(globalId))
                    {
                        timePoints.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation
    }
}
