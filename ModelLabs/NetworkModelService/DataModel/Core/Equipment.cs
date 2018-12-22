﻿using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Equipment : PowerSystemResource
    {
        private bool aggregate;
        private bool normallyInService;

        public Equipment(long globalId) : base(globalId)
        {
        }

        #region Properties
        public bool Aggregate
        {
            get
            {
                return aggregate;
            }

            set
            {
                aggregate = value;
            }
        }

        public bool NormallyInService
        {
            get
            {
                return normallyInService;
            }

            set
            {
                normallyInService = value;
            }
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Equipment x = (Equipment)obj;
                return ((x.aggregate == this.aggregate) &&
                        (x.normallyInService == this.normallyInService));
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

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.EQUIPMENT_AGGREGATE:
                case ModelCode.EQUIPMENT_NORMALLYINSERVICE:

                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.EQUIPMENT_AGGREGATE:
                    property.SetValue(aggregate);
                    break;

                case ModelCode.EQUIPMENT_NORMALLYINSERVICE:
                    property.SetValue(normallyInService);
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
                case ModelCode.EQUIPMENT_AGGREGATE:
                    aggregate = property.AsBool();
                    break;

                case ModelCode.EQUIPMENT_NORMALLYINSERVICE:
                    normallyInService = property.AsBool();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation
    }
}
