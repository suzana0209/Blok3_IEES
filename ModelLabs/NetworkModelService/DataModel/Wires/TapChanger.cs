using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class TapChanger : PowerSystemResource
    {
        private List<long> tapSchedule = new List<long>();

        public TapChanger(long globalId) : base(globalId)
        {
        }

        public List<long> TapSchedule { get => tapSchedule; set => tapSchedule = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                TapChanger x = (TapChanger)obj;
                return (CompareHelper.CompareLists(x.tapSchedule, this.tapSchedule));
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

        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.TAPCHANGER_TAPSCHEDULES:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TAPCHANGER_TAPSCHEDULES:
                    property.SetValue(tapSchedule);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        //public override void SetProperty(Property property)
        //{
        //    switch (property.Id)
        //    {
        //        case ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE:
        //            dayType = property.AsReference();
        //            break;

        //        default:
        //            base.SetProperty(property);
        //            break;
        //    }
        //}

        #endregion IAccess implementation

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return (tapSchedule.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (tapSchedule != null && tapSchedule.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TAPCHANGER_TAPSCHEDULES] = tapSchedule.GetRange(0, tapSchedule.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TAPSCHEDULE_TAPCHANGER:
                    tapSchedule.Add(globalId);
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
                case ModelCode.TAPSCHEDULE_TAPCHANGER:

                    if (tapSchedule.Contains(globalId))
                    {
                        tapSchedule.Remove(globalId);
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
