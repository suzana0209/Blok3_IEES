﻿using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.LoadModel
{
    public class DayType : IdentifiedObject
    {
        private List<long> seasonDayTypeSchedules = new List<long>();

        public DayType(long globalId) : base(globalId)
        {
        }

        public List<long> SeasonDayTypeSchedules { get => seasonDayTypeSchedules; set => seasonDayTypeSchedules = value; }

        public override bool Equals(object obj)
        {
            DayType x = (DayType)obj;
            if (base.Equals(obj))
            {
                return (CompareHelper.CompareLists(x.seasonDayTypeSchedules, this.seasonDayTypeSchedules));
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
                case ModelCode.DAYTYPE_SEASONDAYTYPESCHEDULES:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.DAYTYPE_SEASONDAYTYPESCHEDULES:
                    prop.SetValue(seasonDayTypeSchedules);
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
                return (seasonDayTypeSchedules.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (seasonDayTypeSchedules != null && seasonDayTypeSchedules.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.DAYTYPE_SEASONDAYTYPESCHEDULES] = seasonDayTypeSchedules.GetRange(0, seasonDayTypeSchedules.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE:
                    seasonDayTypeSchedules.Add(globalId);
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
                case ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE:

                    if (seasonDayTypeSchedules.Contains(globalId))
                    {
                        seasonDayTypeSchedules.Remove(globalId);
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
