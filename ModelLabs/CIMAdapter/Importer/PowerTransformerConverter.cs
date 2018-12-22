namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

        #region Populate ResourceDescription

        //Sva  polja i refenrence(GetMapped GID-a), ALI NE I LISTE REFERENCI, 
        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.AliasNameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
                }
            }
        }

        public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cimBasicIntervalSchedule, ResourceDescription rd)
        {
            if ((cimBasicIntervalSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimBasicIntervalSchedule, rd);

                if (cimBasicIntervalSchedule.StartTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_STARTTIME, cimBasicIntervalSchedule.StartTime));
                }
                if (cimBasicIntervalSchedule.Value1MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPLIER, (short)(cimBasicIntervalSchedule.Value1Multiplier)));
                }
                if (cimBasicIntervalSchedule.Value1UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE1UNIT, (short)(cimBasicIntervalSchedule.Value1Unit)));
                }
                if (cimBasicIntervalSchedule.Value2MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPLIER, (short)(cimBasicIntervalSchedule.Value2Multiplier)));
                }
                if (cimBasicIntervalSchedule.Value2UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_VALUE2UNIT, (short)(cimBasicIntervalSchedule.Value2Unit)));
                }
            }
        }

        public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd)
        {
            if ((cimPowerSystemResource != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
            }
        }

        public static void PopulateRegularIntervalScheduleProperties(FTN.RegularIntervalSchedule cimRegIntervalSchedule, ResourceDescription rd)
        {
            if ((cimRegIntervalSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(cimRegIntervalSchedule, rd);
            }
        }

        public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd);

                if (cimEquipment.AggregateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
                }
                if (cimEquipment.NormallyInServiceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMALLYINSERVICE, cimEquipment.NormallyInService));
                }
            }
        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd)
        {
            if ((cimConductingEquipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd);
            }
        }

        public static void PopulateSwitchProperties(FTN.Switch cimSwitch, ResourceDescription rd)
        {
            if ((cimSwitch != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateConductingEquipmentProperties(cimSwitch, rd);

                if (cimSwitch.NormalOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_NORMALOPEN, cimSwitch.NormalOpen));
                }
                if (cimSwitch.RatedCurrentHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RATEDCURRENT, cimSwitch.RatedCurrent));
                }
                if (cimSwitch.RetainedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RATAINED, cimSwitch.Retained));
                }
                if (cimSwitch.SwitchOnCountHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_SWITCHONCOUNT, cimSwitch.SwitchOnCount));
                }
                if (cimSwitch.SwitchOnDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_SWITCHONDATE, cimSwitch.SwitchOnDate));
                }
            }
        }

        public static void PopulateRegularTimePointProperties(FTN.RegularTimePoint cimRegTimePoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegTimePoint != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimRegTimePoint, rd);

                if (cimRegTimePoint.SequenceNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_SEQUENCENUMBER, cimRegTimePoint.SequenceNumber));
                }
                if (cimRegTimePoint.Value1HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_VALUE1, cimRegTimePoint.Value1));
                }
                if (cimRegTimePoint.Value2HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_VALUE2, cimRegTimePoint.Value2));
                }

                if (cimRegTimePoint.IntervalScheduleHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRegTimePoint.IntervalSchedule.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRegTimePoint.GetType().ToString()).Append(" rdfID = \"").Append(cimRegTimePoint.ID);
                        report.Report.Append("\" - Failed to set reference to RegularTimePoint: rdfID \"").Append(cimRegTimePoint.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_REGULARINTERVALSCHEDULE, gid));
                }
            }
        }

        public static void PopulateDayTypeProperties(FTN.DayType cimDayType, ResourceDescription rd)
        {
            if ((cimDayType != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimDayType, rd);
            }
        }

        public static void PopulateSeasonDayTypeScheduleProperties(FTN.SeasonDayTypeSchedule cimSeasonDTypeSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSeasonDTypeSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateRegularIntervalScheduleProperties(cimSeasonDTypeSchedule, rd);

                if (cimSeasonDTypeSchedule.DayTypeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSeasonDTypeSchedule.DayType.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSeasonDTypeSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSeasonDTypeSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to SeasonDayType Schedule: rdfID \"").Append(cimSeasonDTypeSchedule.DayType.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE, gid));
                }
            }
        }

        public static void PopulateTapScheduleProperties(FTN.TapSchedule cimTapSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTapSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateSeasonDayTypeScheduleProperties(cimTapSchedule, rd, importHelper, report);

                if (cimTapSchedule.TapChangerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTapSchedule.TapChanger.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTapSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimTapSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to TapSchedule: rdfID \"").Append(cimTapSchedule.TapChanger.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TAPSCHEDULE_TAPCHANGER, gid));
                }
            }
        }

        public static void PopulateRegulationScheduleProperties(FTN.RegulationSchedule cimRegulationSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegulationSchedule != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateSeasonDayTypeScheduleProperties(cimRegulationSchedule, rd, importHelper, report);

                if (cimRegulationSchedule.RegulatingControlHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRegulationSchedule.RegulatingControl.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRegulationSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimRegulationSchedule.ID);
                        report.Report.Append("\" - Failed to set reference to RegulationSchedule: rdfID \"").Append(cimRegulationSchedule.RegulatingControl.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGULATIONSCHEDULE_REGULATINGCONTROL, gid));
                }
            }
        }

        public static void PopulateTapChangerProperties(FTN.TapChanger cimTapChanger, ResourceDescription rd)
        {
            if ((cimTapChanger != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimTapChanger, rd);
            }
        }

        public static void PopulateRegulatingControlProperties(FTN.RegulatingControl cimRegControl, ResourceDescription rd)
        {
            if ((cimRegControl != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimRegControl, rd);

                if (cimRegControl.DiscreteHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_DISCRETE, cimRegControl.Discrete));
                }
                if (cimRegControl.ModeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_MODE, (short)(cimRegControl.Mode)));
                }
                if (cimRegControl.MonitoredPhaseHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_MONITOREDPHASE, (short)GetDMSPhaseCode(cimRegControl.MonitoredPhase)));
                }
                if (cimRegControl.TargetRangeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_TARGETRANGE, cimRegControl.TargetRange));
                }
                if (cimRegControl.TargetValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCONTROL_TARGETVALUE, cimRegControl.TargetValue));
                }
            }
        }


        #endregion Populate ResourceDescription

        #region Enums convert

        public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier unit)
        {
            switch (unit)
            {
                case FTN.UnitMultiplier.p:
                    return UnitMultiplier.p;
                case FTN.UnitMultiplier.n:
                    return UnitMultiplier.n;
                case FTN.UnitMultiplier.micro:
                    return UnitMultiplier.micro;
                case FTN.UnitMultiplier.m:
                    return UnitMultiplier.m;
                case FTN.UnitMultiplier.c:
                    return UnitMultiplier.c;
                case FTN.UnitMultiplier.d:
                    return UnitMultiplier.d;
                case FTN.UnitMultiplier.k:
                    return UnitMultiplier.k;
                case FTN.UnitMultiplier.M:
                    return UnitMultiplier.M;
                case FTN.UnitMultiplier.G:
                    return UnitMultiplier.G;
                case FTN.UnitMultiplier.T:
                    return UnitMultiplier.T;

                default: return UnitMultiplier.none;
            }
        }

        public static RegulatingControlModelKind GetDMSModelKind(RegulatingControlModeKind modelKind)
        {
            switch (modelKind)
            {
                case FTN.RegulatingControlModeKind.voltage:
                    return RegulatingControlModelKind.volatage;
                case FTN.RegulatingControlModeKind.activePower:
                    return RegulatingControlModelKind.activePower;
                case FTN.RegulatingControlModeKind.reactivePower:
                    return RegulatingControlModelKind.reactivePower;
                case FTN.RegulatingControlModeKind.currentFlow:
                    return RegulatingControlModelKind.currentFlow;
                case FTN.RegulatingControlModeKind.@fixed:
                    return RegulatingControlModelKind.Fixed;
                case FTN.RegulatingControlModeKind.admittance:
                    return RegulatingControlModelKind.admittance;
                case FTN.RegulatingControlModeKind.timeScheduled:
                    return RegulatingControlModelKind.timeScheduled;
                case FTN.RegulatingControlModeKind.temperature:
                    return RegulatingControlModelKind.temperature;
                case FTN.RegulatingControlModeKind.powerFactor:
                    return RegulatingControlModelKind.powerFactor;
                default:
                    return RegulatingControlModelKind.none;
            }
        }

        public static UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol unit)
        {
            switch (unit)
            {
                case FTN.UnitSymbol.VA:
                    return UnitSymbol.VA;
                case FTN.UnitSymbol.W:
                    return UnitSymbol.W;
                case FTN.UnitSymbol.VAr:
                    return UnitSymbol.VAr;
                case FTN.UnitSymbol.VAh:
                    return UnitSymbol.VAh;
                case FTN.UnitSymbol.Wh:
                    return UnitSymbol.Wh;
                case FTN.UnitSymbol.VArh:
                    return UnitSymbol.VArh;
                case FTN.UnitSymbol.V:
                    return UnitSymbol.V;
                case FTN.UnitSymbol.ohm:
                    return UnitSymbol.ohm;
                case FTN.UnitSymbol.A:
                    return UnitSymbol.A;
                case FTN.UnitSymbol.F:
                    return UnitSymbol.F;
                case FTN.UnitSymbol.H:
                    return UnitSymbol.H;
                case FTN.UnitSymbol.degC:
                    return UnitSymbol.degC;
                case FTN.UnitSymbol.s:
                    return UnitSymbol.s;
                case FTN.UnitSymbol.min:
                    return UnitSymbol.min;
                case FTN.UnitSymbol.h:
                    return UnitSymbol.h;
                case FTN.UnitSymbol.deg:
                    return UnitSymbol.deg;
                case FTN.UnitSymbol.rad:
                    return UnitSymbol.rad;
                case FTN.UnitSymbol.J:
                    return UnitSymbol.J;
                case FTN.UnitSymbol.N:
                    return UnitSymbol.N;
                case FTN.UnitSymbol.S:
                    return UnitSymbol.S;
                case FTN.UnitSymbol.Hz:
                    return UnitSymbol.Hz;
                case FTN.UnitSymbol.g:
                    return UnitSymbol.g;
                case FTN.UnitSymbol.Pa:
                    return UnitSymbol.Pa;
                case FTN.UnitSymbol.m:
                    return UnitSymbol.m;
                case FTN.UnitSymbol.m2:
                    return UnitSymbol.m2;
                case FTN.UnitSymbol.m3:
                    return UnitSymbol.m3;
                default:
                    return UnitSymbol.none;
            }
        }

        public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
		{
			switch (phases)
			{
				case FTN.PhaseCode.A:
					return PhaseCode.A;
				case FTN.PhaseCode.AB:
					return PhaseCode.AB;
				case FTN.PhaseCode.ABC:
					return PhaseCode.ABC;
				case FTN.PhaseCode.ABCN:
					return PhaseCode.ABCN;
				case FTN.PhaseCode.ABN:
					return PhaseCode.ABN;
				case FTN.PhaseCode.AC:
					return PhaseCode.AC;
				case FTN.PhaseCode.ACN:
					return PhaseCode.ACN;
				case FTN.PhaseCode.AN:
					return PhaseCode.AN;
				case FTN.PhaseCode.B:
					return PhaseCode.B;
				case FTN.PhaseCode.BC:
					return PhaseCode.BC;
				case FTN.PhaseCode.BCN:
					return PhaseCode.BCN;
				case FTN.PhaseCode.BN:
					return PhaseCode.BN;
				case FTN.PhaseCode.C:
					return PhaseCode.C;
				case FTN.PhaseCode.CN:
					return PhaseCode.CN;
				case FTN.PhaseCode.N:
					return PhaseCode.N;
				case FTN.PhaseCode.s12N:
					return PhaseCode.ABN;
				case FTN.PhaseCode.s1N:
					return PhaseCode.AN;
				case FTN.PhaseCode.s2N:
					return PhaseCode.BN;
				default: return PhaseCode.Unknown;
			}
		}

		#endregion Enums convert
	}
}
