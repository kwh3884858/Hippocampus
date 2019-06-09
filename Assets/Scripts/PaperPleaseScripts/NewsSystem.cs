using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsSystem
{
	Dictionary<string, string> m_news;

	public NewsSystem ()
	{

		m_news = new Dictionary<string, string> ();
		m_news.Add ("明日避难所遭控制", "明日避难所发来讯息，激进宗教团体心火启发会已经控制了明日避难所的议会及情报部门。——请不要允许任何持有明日城身份证件的人士进入避难所。");
		m_news.Add ("巨大旋风正在接近避难所", "南极站昨日观测到一股史无前例的巨大旋风正在形成，地质局发出地震红色警告，预计将于3天后到达，建议所有的人员至少在风暴前一天结束工作，滞留家在不要外出。建筑与设施安全部门已经启动一级相应预案，预计旋风将导致边境区域稳定结构损坏，部分地区面临坍塌风险。");
		m_news.Add ("发送即时信息将需要进行生物特征识别", "根据第二十二号行政令，信息与社会安全部今日宣布避难所所有实体使用即时通讯软件时需实时进行生物特征识别，旨在保护信息安全和社会稳定，这意味目前所有的通讯工具都将受到严格管理，信息与社会安全部相关发言人指出，通过新规，将进一步保护避难所成员的人身与财产安全。");


		m_news.Add ("粉尘爆炸", "避难所三层的一件食品密封加工厂爆发了粉尘爆炸事件，数名熟练技工遭受中度烧伤——避难所的粮食指数将会迎来一次大幅下降，当门外人具备足够的工厂指数或者携带着足量粮食物资时，请酌情使其加入。");
		m_news.Add ("冰崖避难所失联", "冰崖避难所已经持续三天无应答，按照先前消息无疑表明，他们已经被他们自己研发的机器人劳工所推翻，这些机器人残酷狡诈，可以通过图灵测试，但是在使用的骨架比起同体型的人类来说要高——请注意门外人的体重数据。");
		m_news.Add ("对明日避难所的身份限制解禁", "明日避难所目前的执政权已经由避难所内的工会从心火启发会手中夺回。当信众们发现关闭城市动力炉之后，并不能用“心火”使得自己身体发暖时，那位小丑一般的教主就成为了重启动力炉时候的第一铲燃料。");

	}

	public string GetNews (string index)
	{
		return m_news [index];
	}
	public string GetContent (int index)
	{
		int i = 0;
		foreach (string s in m_news.Values) { if (i == index) { return s; } i++; }
		return null;
	}
	public string GetTitle (int index)
	{
		int i = 0;
		foreach (string s in m_news.Keys) { if (i == index) { return s; } i++; }
		return null;
	}

	public List<string> GetAllTitile ()
	{
		return new List<string> (this.m_news.Keys);
	}


}

