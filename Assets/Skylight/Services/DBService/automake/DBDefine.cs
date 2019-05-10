using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skylight
{
	public enum TableType
	{
		TT_Example,
		TT_Model,
		TT_Soldier,
		TT_Amount
	};

	public enum Model_Type
	{
		ARCHER	=	1,//弓箭手
		CATAPULT	=	2,//投石车
		CAVALRY	=	3,//骑兵
		HEAVY_INFANTRY	=	4,//步兵
		LIGHT_INFANTRY	=	5,//枪兵
	};

	public class DBData_Example : DBRecord
	{
		public override void Parse(string[] cols)
		{
			fileds = new object[cols.Length];
			fileds[0] = int.Parse(cols[0]);
			fileds[1] = cols[1];
			fileds[2] = int.Parse(cols[2]);
			fileds[3] = float.Parse(cols[3]);
		}
		public int ID { get { return (int)fileds[0]; } }//编号
		public string strValue { get { return (string)fileds[1]; } }//字符值
		public int IntValue { get { return (int)fileds[2]; } }//整型值
		public float FloatValue { get { return (float)fileds[3]; } }//浮点值
	};

	public class DBData_Model : DBRecord
	{
		public override void Parse(string[] cols)
		{
			fileds = new object[cols.Length];
			fileds[0] = int.Parse(cols[0]);
			fileds[1] = cols[1];
			fileds[2] = cols[2];
			fileds[3] = cols[3];
			fileds[4] = cols[4];
			fileds[5] = cols[5];
			fileds[6] = cols[6];
			fileds[7] = cols[7];
			fileds[8] = cols[8];
			fileds[9] = cols[9];
			fileds[10] = cols[10];
			fileds[11] = cols[11];
			fileds[12] = cols[12];
		}
		public int ID { get { return (int)fileds[0]; } }//编号
		public string Name { get { return (string)fileds[1]; } }//名字
		public string Fbx { get { return (string)fileds[2]; } }//模型
		public string Idle { get { return (string)fileds[3]; } }//闲置
		public string FightIdle { get { return (string)fileds[4]; } }//战斗警备
		public string Walk { get { return (string)fileds[5]; } }//走路
		public string FightWalk { get { return (string)fileds[6]; } }//战斗走路
		public string Run { get { return (string)fileds[7]; } }//跑步
		public string FightRun { get { return (string)fileds[8]; } }//战斗跑步
		public string AttackA { get { return (string)fileds[9]; } }//攻击A
		public string AttackB { get { return (string)fileds[10]; } }//攻击B
		public string DeathA { get { return (string)fileds[11]; } }//死亡A
		public string DeathB { get { return (string)fileds[12]; } }//死亡B
	};

	public class DBData_Soldier : DBRecord
	{
		public override void Parse(string[] cols)
		{
			fileds = new object[cols.Length];
			fileds[0] = int.Parse(cols[0]);
			fileds[1] = int.Parse(cols[1]);
			fileds[2] = int.Parse(cols[2]);
			fileds[3] = float.Parse(cols[3]);
			fileds[4] = float.Parse(cols[4]);
			fileds[5] = int.Parse(cols[5]);
			fileds[6] = int.Parse(cols[6]);
			fileds[7] = float.Parse(cols[7]);
			fileds[8] = float.Parse(cols[8]);
		}
		public int ID { get { return (int)fileds[0]; } }//编号
		public int type { get { return (int)fileds[1]; } }//模型类型
		public int hp { get { return (int)fileds[2]; } }//血量
		public float range { get { return (float)fileds[3]; } }//队伍射程
		public float range2 { get { return (float)fileds[4]; } }//士兵射程
		public int attack { get { return (int)fileds[5]; } }//攻击力
		public int defend { get { return (int)fileds[6]; } }//防御力
		public float speed { get { return (float)fileds[7]; } }//移动速度
		public float cdtime { get { return (float)fileds[8]; } }//攻击间隔
	};


};

