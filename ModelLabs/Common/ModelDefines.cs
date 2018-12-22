using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{
	
	public enum DMSType : short
	{		
		MASK_TYPE							= unchecked((short)0xFFFF),

        SWITCH                              = 0x0001,
        REGULATINGCONTROL                   = 0x0002,
        TAPCHANGER                          = 0x0003,
        DAYTYPE                             = 0x0004,
        REGULARTIMEPOINT                    = 0x0005,
        TAPSCHEDULE                         = 0x0006,
        REGULATIONSCHEDULE                  = 0x0007
    }

    [Flags]
	public enum ModelCode : long
	{
        IDOBJ                                   = 0x1000000000000000,
        IDOBJ_GID                               = 0x1000000000000104,//Int64 
        IDOBJ_ALIASNAME                         = 0x1000000000000207,
        IDOBJ_MRID                              = 0x1000000000000307,
        IDOBJ_NAME                              = 0x1000000000000407,

        BASICINTERVALSCHEDULE                   = 0x1100000000000000,
        BASICINTERVALSCHEDULE_STARTTIME         = 0x1100000000000108,
        BASICINTERVALSCHEDULE_VALUE1MULTIPLIER  = 0x110000000000020a,
        BASICINTERVALSCHEDULE_VALUE1UNIT        = 0x110000000000030a,
        BASICINTERVALSCHEDULE_VALUE2MULTIPLIER  = 0x110000000000040a,
        BASICINTERVALSCHEDULE_VALUE2UNIT        = 0x110000000000050a,

        REGULARTIMEPOINT                        = 0x1200000000050000,
        REGULARTIMEPOINT_SEQUENCENUMBER         = 0x1200000000050103,
        REGULARTIMEPOINT_VALUE1                 = 0x1200000000050205,
        REGULARTIMEPOINT_VALUE2                 = 0x1200000000050305,
        REGULARTIMEPOINT_REGULARINTERVALSCHEDULE = 0x1200000000050409,

        POWERSYSTEMRESOURCE                     = 0x1300000000000000,

        DAYTYPE                                 = 0x1400000000040000,
        DAYTYPE_SEASONDAYTYPESCHEDULES          = 0x1400000000040119,

        REGULARINTERVALSCHEDULE                 = 0x1110000000000000,
        REGULARINTERVALSCHEDULE_REGULARTIMEPOINTS = 0x1110000000000119,

        SEASONDAYTYPESCHEDULE                   = 0x1111000000000000,
        SEASONDAYTYPESCHEDULE_DAYTYPE           = 0x1111000000000109,

        REGULATIONSCHEDULE                      = 0x1111200000070000,
        REGULATIONSCHEDULE_REGULATINGCONTROL    = 0x1111200000070109,

        TAPSCHEDULE                             = 0x1111100000060000,
        TAPSCHEDULE_TAPCHANGER                  = 0x1111100000060109,

        TAPCHANGER                              = 0x1330000000030000,
        TAPCHANGER_TAPSCHEDULES                 = 0x1330000000030119,

        EQUIPMENT                               = 0x1310000000000000,
        EQUIPMENT_AGGREGATE                     = 0x1310000000000101,
        EQUIPMENT_NORMALLYINSERVICE             = 0x1310000000000201,

        CONDACTINGEQUIPMENT                     = 0x1311000000000000,

        SWITCH                                  = 0x1311100000010000,
        SWITCH_NORMALOPEN                       = 0x1311100000010101,
        SWITCH_RATEDCURRENT                     = 0x1311100000010205,
        SWITCH_RATAINED                         = 0x1311100000010301,
        SWITCH_SWITCHONCOUNT                    = 0x1311100000010403,
        SWITCH_SWITCHONDATE                     = 0x1311100000010508,

        REGULATINGCONTROL                       = 0x1320000000020000,
        REGULATINGCONTROL_DISCRETE              = 0x1320000000020101,
        REGULATINGCONTROL_MODE                  = 0x132000000002020a,
        REGULATINGCONTROL_MONITOREDPHASE        = 0x132000000002030a,
        REGULATINGCONTROL_TARGETRANGE           = 0x1320000000020405,
        REGULATINGCONTROL_TARGETVALUE           = 0x1320000000020505,
        REGULATINGCONTROL_REGULATIONSCHEDULES   = 0x1320000000020619,
    }

    [Flags]
	public enum ModelCodeMask : long
	{
		MASK_TYPE			 = 0x00000000ffff0000,
		MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
		MASK_ATTRIBUTE_TYPE	 = 0x00000000000000ff,

		MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
		MASK_FIRSTNBL		  = unchecked((long)0xf000000000000000),
		MASK_DELFROMNBL8	  = unchecked((long)0xfffffff000000000),		
	}																		
}


