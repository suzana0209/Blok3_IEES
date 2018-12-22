using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            //// import all concrete model types (DMSType enum)
            //ImportBaseVoltages();
            //ImportLocations();
            //ImportPowerTransformers();
            //ImportTransformerWindings();
            //ImportWindingTests();

            ImportSwitch();
            ImportRegulatingControl();
            ImportDayType();
            ImportRegulationSchedule(); 
            ImportTapChanger();
            ImportTapSchedule();
            ImportRegularTimePoint();

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

        #region Import
        private void ImportSwitch()
        {
            SortedDictionary<string, object> cimSwitches = concreteModel.GetAllObjectsOfType("FTN.Switch");
            if (cimSwitches != null)
            {
                foreach (KeyValuePair<string, object> item in cimSwitches)
                {
                    FTN.Switch cimSwitch = item.Value as FTN.Switch;

                    ResourceDescription rd = CreateSwitchResourceDescription(cimSwitch);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Switch ID = ").Append(cimSwitch.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("BaseVoltage ID = ").Append(cimSwitch.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateSwitchResourceDescription(FTN.Switch cimSwitch)
        {
            ResourceDescription rd = null;
            if (cimSwitch != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SWITCH,
                    importHelper.CheckOutIndexForDMSType(DMSType.SWITCH));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSwitch.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateSwitchProperties(cimSwitch, rd);
            }
            return rd;
        }

        private void ImportRegulatingControl()
        {
            SortedDictionary<string, object> cimRegControls = concreteModel.GetAllObjectsOfType("FTN.RegulatingControl");
            if (cimRegControls != null)
            {
                foreach (KeyValuePair<string, object> item in cimRegControls)
                {
                    FTN.RegulatingControl cimRegControl = item.Value as FTN.RegulatingControl;

                    ResourceDescription rd = CreateRegControlResourceDescription(cimRegControl);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Regulating Control ID = ").Append(cimRegControl.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Regulating Control ID = ").Append(cimRegControl.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateRegControlResourceDescription(FTN.RegulatingControl cimRegControl)
        {
            ResourceDescription rd = null;
            if (cimRegControl != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATINGCONTROL,
                    importHelper.CheckOutIndexForDMSType(DMSType.REGULATINGCONTROL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimRegControl.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateRegulatingControlProperties(cimRegControl, rd);
            }
            return rd;
        }

        private void ImportTapChanger()
        {
            SortedDictionary<string, object> cimTapChangers = concreteModel.GetAllObjectsOfType("FTN.TapChanger");
            if (cimTapChangers != null)
            {
                foreach (KeyValuePair<string, object> item in cimTapChangers)
                {
                    FTN.TapChanger cimTapChanger = item.Value as FTN.TapChanger;

                    ResourceDescription rd = CreateTapChangerResourceDescription(cimTapChanger);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Switch ID = ").Append(cimTapChanger.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Tap Changer ID = ").Append(cimTapChanger.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateTapChangerResourceDescription(FTN.TapChanger cimTapChanger)
        {
            ResourceDescription rd = null;
            if (cimTapChanger != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TAPCHANGER,
                    importHelper.CheckOutIndexForDMSType(DMSType.TAPCHANGER));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTapChanger.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateTapChangerProperties(cimTapChanger, rd);
            }
            return rd;
        }

        private void ImportDayType()
        {
            SortedDictionary<string, object> cimTapDayType = concreteModel.GetAllObjectsOfType("FTN.DayType");
            if (cimTapDayType != null)
            {
                foreach (KeyValuePair<string, object> item in cimTapDayType)
                {
                    FTN.DayType cimDayType = item.Value as FTN.DayType;

                    ResourceDescription rd = CreateDayTypeResourceDescription(cimDayType);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("DayType ID = ").Append(cimDayType.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("DayType ID = ").Append(cimDayType.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateDayTypeResourceDescription(FTN.DayType cimDayType)
        {
            ResourceDescription rd = null;
            if (cimDayType != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DAYTYPE,
                    importHelper.CheckOutIndexForDMSType(DMSType.DAYTYPE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimDayType.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateDayTypeProperties(cimDayType, rd);
            }
            return rd;
        }

        private void ImportRegularTimePoint()
        {
            SortedDictionary<string, object> cimRegTimePoints = concreteModel.GetAllObjectsOfType("FTN.RegularTimePoint");
            if (cimRegTimePoints != null)
            {
                foreach (KeyValuePair<string, object> item in cimRegTimePoints)
                {
                    FTN.RegularTimePoint cimRegTimePoint = item.Value as FTN.RegularTimePoint;

                    ResourceDescription rd = CreateRegularTimePointResourceDescription(cimRegTimePoint);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Regular Time Point ID = ").Append(cimRegTimePoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Regular time Point ID = ").Append(cimRegTimePoint.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateRegularTimePointResourceDescription(FTN.RegularTimePoint cimRegTimePoint)
        {
            ResourceDescription rd = null;
            if (cimRegTimePoint != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULARTIMEPOINT,
                    importHelper.CheckOutIndexForDMSType(DMSType.REGULARTIMEPOINT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimRegTimePoint.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateRegularTimePointProperties(cimRegTimePoint, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportTapSchedule()
        {
            SortedDictionary<string, object> cimTapSchedules = concreteModel.GetAllObjectsOfType("FTN.TapSchedule");
            if (cimTapSchedules != null)
            {
                foreach (KeyValuePair<string, object> item in cimTapSchedules)
                {
                    FTN.TapSchedule cimTapSchedule = item.Value as FTN.TapSchedule;

                    ResourceDescription rd = CreateTapScheduleResourceDescription(cimTapSchedule);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Tap schedule ID = ").Append(cimTapSchedule.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Tap schedule ID = ").Append(cimTapSchedule.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateTapScheduleResourceDescription(FTN.TapSchedule cimTapSchedule)
        {
            ResourceDescription rd = null;
            if (cimTapSchedule != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TAPSCHEDULE,
                    importHelper.CheckOutIndexForDMSType(DMSType.TAPSCHEDULE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTapSchedule.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateTapScheduleProperties(cimTapSchedule, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportRegulationSchedule()
        {
            SortedDictionary<string, object> cimRegSchedules = concreteModel.GetAllObjectsOfType("FTN.RegulationSchedule");
            if (cimRegSchedules != null)
            {
                foreach (KeyValuePair<string, object> item in cimRegSchedules)
                {
                    FTN.RegulationSchedule cimRegSchedule = item.Value as FTN.RegulationSchedule;

                    ResourceDescription rd = CreateRegulationScheduleResourceDescription(cimRegSchedule);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Regulation schedule ID = ").Append(cimRegSchedule.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Regulation schedule ID = ").Append(cimRegSchedule.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateRegulationScheduleResourceDescription(FTN.RegulationSchedule cimRegulationSchedule)
        {
            ResourceDescription rd = null;
            if (cimRegulationSchedule != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATIONSCHEDULE,
                    importHelper.CheckOutIndexForDMSType(DMSType.REGULATIONSCHEDULE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimRegulationSchedule.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateRegulationScheduleProperties(cimRegulationSchedule, rd, importHelper, report);
            }
            return rd;
        }


        //private ResourceDescription CreateLocationResourceDescription(FTN.Location cimLocation)
        //{
        //    ResourceDescription rd = null;
        //    if (cimLocation != null)
        //    {
        //        long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.LOCATION, importHelper.CheckOutIndexForDMSType(DMSType.LOCATION));
        //        rd = new ResourceDescription(gid);
        //        importHelper.DefineIDMapping(cimLocation.ID, gid);

        //        ////populate ResourceDescription
        //        PowerTransformerConverter.PopulateLocationProperties(cimLocation, rd);
        //    }
        //    return rd;
        //}

        //private void ImportPowerTransformers()
        //{
        //    SortedDictionary<string, object> cimPowerTransformers = concreteModel.GetAllObjectsOfType("FTN.PowerTransformer");
        //    if (cimPowerTransformers != null)
        //    {
        //        foreach (KeyValuePair<string, object> cimPowerTransformerPair in cimPowerTransformers)
        //        {
        //            FTN.PowerTransformer cimPowerTransformer = cimPowerTransformerPair.Value as FTN.PowerTransformer;

        //            ResourceDescription rd = CreatePowerTransformerResourceDescription(cimPowerTransformer);
        //            if (rd != null)
        //            {
        //                delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
        //                report.Report.Append("PowerTransformer ID = ").Append(cimPowerTransformer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
        //            }
        //            else
        //            {
        //                report.Report.Append("PowerTransformer ID = ").Append(cimPowerTransformer.ID).AppendLine(" FAILED to be converted");
        //            }
        //        }
        //        report.Report.AppendLine();
        //    }
        //}

        //private ResourceDescription CreatePowerTransformerResourceDescription(FTN.PowerTransformer cimPowerTransformer)
        //{
        //    ResourceDescription rd = null;
        //    if (cimPowerTransformer != null)
        //    {
        //        long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTR, importHelper.CheckOutIndexForDMSType(DMSType.POWERTR));
        //        rd = new ResourceDescription(gid);
        //        importHelper.DefineIDMapping(cimPowerTransformer.ID, gid);

        //        ////populate ResourceDescription
        //        PowerTransformerConverter.PopulatePowerTransformerProperties(cimPowerTransformer, rd, importHelper, report);
        //    }
        //    return rd;
        //}

        //private void ImportTransformerWindings()
        //{
        //    SortedDictionary<string, object> cimTransformerWindings = concreteModel.GetAllObjectsOfType("FTN.TransformerWinding");
        //    if (cimTransformerWindings != null)
        //    {
        //        foreach (KeyValuePair<string, object> cimTransformerWindingPair in cimTransformerWindings)
        //        {
        //            FTN.TransformerWinding cimTransformerWinding = cimTransformerWindingPair.Value as FTN.TransformerWinding;

        //            ResourceDescription rd = CreateTransformerWindingResourceDescription(cimTransformerWinding);
        //            if (rd != null)
        //            {
        //                delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
        //                report.Report.Append("TransformerWinding ID = ").Append(cimTransformerWinding.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
        //            }
        //            else
        //            {
        //                report.Report.Append("TransformerWinding ID = ").Append(cimTransformerWinding.ID).AppendLine(" FAILED to be converted");
        //            }
        //        }
        //        report.Report.AppendLine();
        //    }
        //}

        //private ResourceDescription CreateTransformerWindingResourceDescription(FTN.TransformerWinding cimTransformerWinding)
        //{
        //    ResourceDescription rd = null;
        //    if (cimTransformerWinding != null)
        //    {
        //        long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTRWINDING, importHelper.CheckOutIndexForDMSType(DMSType.POWERTRWINDING));
        //        rd = new ResourceDescription(gid);
        //        importHelper.DefineIDMapping(cimTransformerWinding.ID, gid);

        //        ////populate ResourceDescription
        //        PowerTransformerConverter.PopulateTransformerWindingProperties(cimTransformerWinding, rd, importHelper, report);
        //    }
        //    return rd;
        //}

        //private void ImportWindingTests()
        //{
        //    SortedDictionary<string, object> cimWindingTests = concreteModel.GetAllObjectsOfType("FTN.WindingTest");
        //    if (cimWindingTests != null)
        //    {
        //        foreach (KeyValuePair<string, object> cimWindingTestPair in cimWindingTests)
        //        {
        //            FTN.WindingTest cimWindingTest = cimWindingTestPair.Value as FTN.WindingTest;

        //            ResourceDescription rd = CreateWindingTestResourceDescription(cimWindingTest);
        //            if (rd != null)
        //            {
        //                delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
        //                report.Report.Append("WindingTest ID = ").Append(cimWindingTest.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
        //            }
        //            else
        //            {
        //                report.Report.Append("WindingTest ID = ").Append(cimWindingTest.ID).AppendLine(" FAILED to be converted");
        //            }
        //        }
        //        report.Report.AppendLine();
        //    }
        //}

        //private ResourceDescription CreateWindingTestResourceDescription(FTN.WindingTest cimWindingTest)
        //{
        //    ResourceDescription rd = null;
        //    if (cimWindingTest != null)
        //    {
        //        long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.WINDINGTEST, importHelper.CheckOutIndexForDMSType(DMSType.WINDINGTEST));
        //        rd = new ResourceDescription(gid);
        //        importHelper.DefineIDMapping(cimWindingTest.ID, gid);

        //        ////populate ResourceDescription
        //        PowerTransformerConverter.PopulateWindingTestProperties(cimWindingTest, rd, importHelper, report);
        //    }
        //    return rd;
        //}
        #endregion Import
    }
}

