using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class RegulatingControl : PowerSystemResource
    {
        private bool discreate;
        private RegulatingControlModelKind mode;
        private PhaseCode monitoredPhase;
        private float targetRange;
        private float targetValue;
        private List<long> regulationSchedules = new List<long>();

        public RegulatingControl(long globalId) : base(globalId)
        {
        }

        #region Properties
        public bool Discreate { get => discreate; set => discreate = value; }
        public RegulatingControlModelKind Mode { get => mode; set => mode = value; }
        public PhaseCode MonitoredPhase { get => monitoredPhase; set => monitoredPhase = value; }
        public float TargetRange { get => targetRange; set => targetRange = value; }
        public float TargetValue { get => targetValue; set => targetValue = value; }
        public List<long> RegulationSchedules { get => regulationSchedules; set => regulationSchedules = value; }
        #endregion

        public override bool Equals(object obj)
        {
            RegulatingControl x = (RegulatingControl)obj;
            if (base.Equals(obj))
            {
                return (x.discreate == this.discreate && x.mode == this.mode &&
                        x.monitoredPhase == this.monitoredPhase && x.targetRange == this.targetRange &&
                        x.targetValue == this.targetValue &&
                        CompareHelper.CompareLists(x.regulationSchedules, this.regulationSchedules));
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
                case ModelCode.REGULATINGCONTROL_DISCRETE:
                case ModelCode.REGULATINGCONTROL_MODE:
                case ModelCode.REGULATINGCONTROL_MONITOREDPHASE:
                case ModelCode.REGULATINGCONTROL_TARGETRANGE:
                case ModelCode.REGULATINGCONTROL_TARGETVALUE:
                case ModelCode.REGULATINGCONTROL_REGULATIONSCHEDULES:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATINGCONTROL_DISCRETE:
                    property.SetValue(discreate);
                    break;

                case ModelCode.REGULATINGCONTROL_MODE:
                    property.SetValue((short)mode);
                    break;

                case ModelCode.REGULATINGCONTROL_MONITOREDPHASE:
                    property.SetValue((short)monitoredPhase);
                    break;

                case ModelCode.REGULATINGCONTROL_TARGETRANGE:
                    property.SetValue(targetRange);
                    break;

                case ModelCode.REGULATINGCONTROL_TARGETVALUE:
                    property.SetValue(targetValue);
                    break;

                case ModelCode.REGULATINGCONTROL_REGULATIONSCHEDULES:
                    property.SetValue(regulationSchedules);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATINGCONTROL_DISCRETE:
                    discreate = property.AsBool();
                    break;

                case ModelCode.REGULATINGCONTROL_MODE:
                    mode = (RegulatingControlModelKind)property.AsEnum();
                    break;

                case ModelCode.REGULATINGCONTROL_MONITOREDPHASE:
                    monitoredPhase = (PhaseCode)property.AsEnum();
                    break;

                case ModelCode.REGULATINGCONTROL_TARGETRANGE:
                    targetRange = property.AsFloat();
                    break;

                case ModelCode.REGULATINGCONTROL_TARGETVALUE:
                    targetValue = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return (regulationSchedules.Count > 0) || base.IsReferenced;
            }
        }


        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (regulationSchedules != null && regulationSchedules.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULATINGCONTROL_REGULATIONSCHEDULES] = regulationSchedules.GetRange(0, regulationSchedules.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL:
                    regulationSchedules.Add(globalId);
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
                case ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL:

                    if (regulationSchedules.Contains(globalId))
                    {
                        regulationSchedules.Remove(globalId);
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
