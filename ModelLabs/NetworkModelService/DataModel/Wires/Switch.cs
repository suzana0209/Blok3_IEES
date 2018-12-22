using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
        private bool normalOpen;
        private float ratedCurrent;
        private bool retained;
        private int switchOnCount;
        private DateTime switchOnDate;

        #region Properities
        public bool NormalOpen { get => normalOpen; set => normalOpen = value; }
        public float RatedCurrent { get => ratedCurrent; set => ratedCurrent = value; }
        public bool Retained { get => retained; set => retained = value; }
        public int SwitchOnCount { get => switchOnCount; set => switchOnCount = value; }
        public DateTime SwitchOnDate { get => switchOnDate; set => switchOnDate = value; }

        #endregion

        public Switch(long globalId) : base(globalId) { }


        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Switch x = (Switch)obj;
                return (x.normalOpen == this.normalOpen &&
                        x.ratedCurrent == this.ratedCurrent &&
                        x.retained == this.retained &&
                        x.switchOnCount == this.switchOnCount &&
                        x.switchOnDate == this.switchOnDate);
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
                case ModelCode.SWITCH_NORMALOPEN:
                case ModelCode.SWITCH_RATEDCURRENT:
                case ModelCode.SWITCH_RATAINED:
                case ModelCode.SWITCH_SWITCHONCOUNT:
                case ModelCode.SWITCH_SWITCHONDATE:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    prop.SetValue(normalOpen);
                    break;

                case ModelCode.SWITCH_RATEDCURRENT:
                    prop.SetValue(ratedCurrent);
                    break;

                case ModelCode.SWITCH_RATAINED:
                    prop.SetValue(retained);
                    break;

                case ModelCode.SWITCH_SWITCHONCOUNT:
                    prop.SetValue(switchOnCount);
                    break;

                case ModelCode.SWITCH_SWITCHONDATE:
                    prop.SetValue(switchOnDate);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    normalOpen = property.AsBool();
                    break;
                case ModelCode.SWITCH_RATEDCURRENT:
                    ratedCurrent = property.AsFloat();
                    break;
                case ModelCode.SWITCH_RATAINED:
                    retained = property.AsBool();
                    break;
                case ModelCode.SWITCH_SWITCHONCOUNT:
                    switchOnCount = property.AsInt();
                    break;
                case ModelCode.SWITCH_SWITCHONDATE:
                    switchOnDate = property.AsDateTime();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion
    }
}
