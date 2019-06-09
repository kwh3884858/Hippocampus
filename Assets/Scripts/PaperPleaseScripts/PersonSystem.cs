using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSystem
{


	List<Person> m_day0;
	List<Person> m_day1;
	List<Person> m_day2;


	PersonSystem ()
	{
		m_day0 = new List<Person> ();

		Person person;

		person = new Person ();
		person.m_name = "克托卡里斯列夫斯基";
		person.m_year = 40;
		person.m_description = "他看上去风尘仆仆，身上的夹克已经磨破了边角，露出黑色的棉絮";
		person.m_intro = new List<string>{
		"您好，先生",
		"我想进入避难所,我和我的家人已经在路上颠簸了一个月",
		"我们精疲力尽",
		"但是只要能让我们休息几天，您就会发现我是一名绝佳的蒸汽引擎工程师" };

		person.m_argue = new List<string> {
			"长官",
			"长官，我求求您，我不知道发生了什么，但是我在路上听到了一些传闻。",
			"我怎么样都好，请您对我的妻女网开一面，她们没法再坚持下去了。",
				"求你了，她们就在后面，请一定要让她们过去"
		};

		person.m_endPass = new List<string>
		{ "求你了，一定要让她们过来，她们单独在外面是活不下去。" };

		person.m_endBlocks = new List<string> {
			"请一定要让她们通过，求你了！外面已经是地狱了"
		};

		person.m_questions = new List<string> {
			"明日避难所身份证"
		};

		person.m_answers = new List<string> { "" };

		m_day0.Add (person);

		person = new Person ();

		person.m_name = "凯瑟琳";
		person.m_year = 30;
		person.m_description = "";
		person.m_intro = new List<string> { };
		person.m_argue = new List<string> { };
		person.m_endPass = new List<string> { };
		person.m_questions = new List<string> { };
		person.m_answers = new List<string> { };

		m_day0.Add (person);


		person.m_name = "凯瑟琳";
		person.m_year = 30;
		person.m_description = "";
		person.m_intro = new List<string> { };
		person.m_argue = new List<string> { };
		person.m_endPass = new List<string> { };
		person.m_questions = new List<string> { };
		person.m_answers = new List<string> { };

		m_day0.Add (person);


		person.m_name = "凯瑟琳";
		person.m_year = 30;
		person.m_description = "";
		person.m_intro = new List<string> { };
		person.m_argue = new List<string> { };
		person.m_endPass = new List<string> { };
		person.m_questions = new List<string> { };
		person.m_answers = new List<string> { };

		m_day0.Add (person);



	}
}
