using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StoryText : MonoBehaviour
{

	public string[] sequence1 = { "Amir, Benjamin, and Caleb are worried about their lost friend Imran." };
	public string[] sequence2 = { "They venture into the wilderness to find their lost friend." };
	public string[] sequence3 = { "Only one of the men return to their village." };
	public string[] sequence4 = { "Although the four are lifelong friends, their families do not get along because of religious differences."};
	public string[] sequence5 = { "The survivor family, offended by the accusation, argues with the accusing family and insults their deceased." };
	public string[] sequence6 = { "The two families agree that the survivor must go back into the wilderness." };
	public string[] sequence7 = { "The accusing family demands that the survivor go back to the wild to find the bodies of their deceased. ",
		"The family of the survivor argue and declines."
	};
	public string[] sequence8 = { "He eventually complies..." };
	public string[] sequence9 = { "but does not return." };
	public string[] sequence10 = { "With the harsh insults, the families' anger intensifies" };
	public string[] sequence11 = { "Despite the ill feelings toward eachother, the families mourn for each others' losses." };
	public string[] sequence12 = { "The families respond in anger." };
	public string[] sequence13 = {"Ultimately being forced to go into the wilderness...",
		"...never to return again."
	};
	public string[] sequence14 = {
		"An angry family member follows the survivor family member into the wilderness.",
		"Striking in anger, the man kills an innocent survivor member."
	};
	public string[] sequence15 = { "The Caleb family accuses the survivor of murdering their member." };
    public string[] sequence16 = { "Although initially hostile, the families decide that it is not the best time for anger.", "They mourn together for their losses." };

	public string[] s1_1 = { "The two families have hated the family of the survivor for generations. After their son return fro mthe forest unscathed and their member lost to the wild, they suspect the survivor was instrumental to the death of their son. They plan their revenge." };
	public string[] s1_2 = { "Amir and Caleb's family hates Benjamin's family." };
	public string[] s1_3 = { "The two families plot the murder of Benjamin." };

	public string[] s2_1 = { "Te mortal blood feud between two of the families escalates when only one son return alive. " };
	public string[] s2_3 = { "The family ofthe dead accuses the suvivor of murdering their son." };
	public string[] s2_4 = { "Enraged at this accusation, the survivor's' family insults the memory of their deceased." };
	public string[] s2_5 = { "When the deceased family demeds the survivor return to the forest to search for the others, he refuses. " };
	public string[] s2_6 = { "In his stead, a memberof the deceased's family ventures to the wild." };

	public string[] s4_1 = { "After one son return from the forest, the deceased's families mourn their loss, harboring no ill will to the survivor's family" };

     


    #region PLASENCIATEXT

	public string[] sequence1_0_0 = {
                                        "n",
    "Welcome to the City of Plasencia and the year of Our Lord 1416. (5176 Jewish and 819 Islamic calendar)", "The Spanish Middle Ages were a tense but productive period of coexistence (<i>convivencia</i>) for Jews, Christians, and Muslims. "};
	public string[] sequence1_0_1 = {
    "Warfare, scientific discovery, philosophical inquiry -- and even intermarriage -- were all characteristics of the 800 years of intercommunal life on the Iberian Peninsula."};
	public string[] sequence1_0_2 = {
    "However, since the 1390s, Jews in the Christian Kingdom of Castile and Leon resided in a religiously-charged world that placed their communities in jeopardy."};
	public string[] sequence1_0_3 = {
    "A full century before the infamous <i>Edict of Expulsion of 1492</i>, riots had led to mass expulsion, murder, and conversion of Jews.",
    "And yet -- Plasencia still existed as a typical mixed-faith Castilian city. About 650 souls resided within the city walls. 42% were Jewish, 34% Christian, and 24% Muslim. "};
    public string[] sequence1_0_4 = {"Sometime before 1416, several Jewish families found it desirable -- perhaps necessary -- to live in a gated collection of homes known as the <i>Apartamiento de La Mota</i> (visualized here without its walls). ",
    "Militant evangelization of Jews in the closeby cities of Zamora and Salamanca may have been the impetus for its creation. "};
	public string[] sequence1_0_5 = {
    "In this digital narrative, we re-create the multifaith story of <i>La Mota</i>. It is a tale preserved only on parchment and now lives again in our digital world -- <i>Virtual Plasencia.</i>"};

	public string[] sequence1_0 = {
		"Welcome to Plasencia, the year of Our Lord, one-thousand-four-hundred and sixteen (5176 of the Hebrew calendar and 819 of the Islamic calendar). \n\nEver since the 1390s and the massive anti-Jewish riots that characterized this period, Jews in the Kingdom of Castile and Leon had resided in a religiously-charged environment that placed their communities in economic, religious, and physical jeopardy. ", 
		"Plasencia, ruled directly by Christian King Juan II, was not free of anti-Jewish sentiment. There were serious disruptions to the traditional residential intermixing of Jewish, Christian, and Muslim families, which was a hallmark of co-existence in medieval Spain.", 
		"At the opening of the 15th century, Plasencia was neither exceptional nor unusual because of its multi-religious character. 42% of the city was Jewish, 34% Christian, and 24% Muslim. Yes, the city had a sizeable Jewish population, but Islamic al-Andalus and Christian España had known over seven centuries of interreligious life since 711. ",
		"Sometime before 1416, but after the 1390s, several Jewish families in Plasencia found it desirable to live in a fortified and gated collection of homes known as the Apartamiento de La Mota (visualized here without its walls). This enclosed section of homes was located across from the Church of Saint Nicolás and the Palace of the Mirabeles. ", 
		"Militant Christian evangelization of Jewish communities in the region during the early 15th century may have been the impetus for the creation of La Mota. For example, in 1411, Vicente Ferrer preached in the the close by cities of Zamora and Salamanca. Ferrer went as far as to preach in these communities’ synagogues. Salamanca’s Jewish community disintegrated after this time, but Plasencia’s survived.\n\nIn this digital narrative we re-create the story of La Mota as revealed through the medieval manuscripts of Spain.  ", 
		"How do we know about La Mota and it’s mostly Jewish residency?\n\nAs in most cases of life -- La Mota’s history is revealed through the most ancient of records -- tax and property documents. \n\nYes, death and taxes are certain for all of us."
	};
	public string[] sequence1_1 = {
		"It is 1416. We now join Christian noblemen, scribes, Jewish leaders, and residents. We witness the reawakening of long lost lives. ", 
		"<i>La Mota</i> housed the synagogue, the Jewish homes, and the dwellings of a Christian nobleman -- Tel Díaz de Vega.", 
		"We do not know why Tel Diaz was the sole Christian property owner inside La Mota. His presence suggests that he was a recent Jewish convert to Christianity (<i>a converso</i>). ", 
		"In 1416, the city council forced Tel Díaz to forfeit his property (a large collection of homes) to another Christian nobleman. Why? Tel Díaz had many unpaid debts. ",
        "In April, the city council supervised the liquidation of his holdings in order to settle a personal debt. ",
        "From the archival record, we learn about the residents of <i>La Mota</i>. Almost all were Jewish.",
        "Although Jews did own property in Plasencia, and many held noble titles, this event illustrates the lives of several who leased homes."
	};
	public string[] sequence1_2 = {
		"A complicated series of transactions led to the new owner of Tel Díaz’s houses -- Fernándo de la Mota, or <i>Fernándo of the Hillock</i>",
        "He was one of a series of important owners and the neighborhood assumed his surname -- <i>La Mota</i>. He would not be the last owner and this reality would have dire consequences for <i>La Mota’s</i> Jews",
        "Who lived here?",
        "Rabbi Abraham de Loya",
        "Yucef Castaño",
        "Symuel Abenhabibe",
        "Yuce Abencur",
        "Çag Pardo",
        "and Hayn and Symuel Daza. ",
        "Although Fernándo purchased these homes -- he also encouraged the families to stay."
	};
    public string[] sequence2_0 = {
        "At this moment, Mayor Juan Sánchez and his scribe supervised the transfer of five homes to Fernándo. The Jewish residents joined them.",
        "Due to their interconnected lives, and in spite of the religiously-charged environment, Plasencia’s Jews and Christians demonstrated a fascinating respect for one another.",
        "We now bear witness to simple, but profound events like Fernándo’s taking possession of houses. Specifically, those homes where Jewish families resided."
                                  };

    public string[] sequence3_0 = { 
    
	"<i>According to the scribe:</i>",
    "mc",
    "<i>Fernándo entered into the houses lived in by Yucef Castaño. </i>",
    "<i>These houses, the best of all of them, had been owned by Tel Diaz. </i>",
    "<i>[Fernándo] took possession of the homes by physically walking into them...and then he closed all of its doors.</i>",
    "n",
    "By entering each home for a short period, Fernándo assumed physical ownership of the houses. ",
    "The event demonstrates that entering a dwelling was a ritually and legally significant moment. Further, Jews and Christians came to know each other’s private lives."
                                  };
	public string[] sequence4_0 = { 
		"Next was the house of Symuel and Ledicia Abenabibe.",
        "Although we do not know Symuel’s profession, his relative, Mose, was both a nobleman and a silversmith. Yes, Spain had many Jewish nobles.",
        "Again a ritual transfer of homes occurred. ",
        "The scribe also recorded new details:",
        "mc",
        "<i>He, Férnando, then opened the doors and shook the hands of Ledicia and her husband, Symuel Abenhabibe, and all of \tthe other Jews living in these homes.</i>",
        "n",
        "This exchange of handshakes reveals that Christians and Jews were accustomed to coming into physical contact with each other. ",
        "Human relations were tactile and intimate.",
        "Religious difference did not prevent personal interactions between men and women.",
        "Unfortunately, this religious respect would come to an end in Spain by 1492.",
        "But 1492 had not come to pass and Plasencia remained an interwoven interreligious landscape with many “ups” and “downs” to come."
	};
	public string[] sequence5_1 = { 
        "We now arrive at the home of Rabbi Abraham de Loya and view another property transfer.",
        "It is remarkable to learn that Rabbi Abraham appears to have shaken hands with Fernándo -- a clear indication that he was accustomed to physical contact with other faith groups.",
        "Here, again, we see that the boundaries of religious ritual purity were routinely breached.",
        "<i>Convivencia</i> in Plasencia was personal.",
        "We are now at the heart of the Jewish quarter of Plasencia and at its synagogue. ",
        "Jewish life revolved around this community center and place of worship.",
        "Today the building no longer exists...but in <i>Virtual Plasencia</i> the synagogue still breathes vigor into the city."
	};
	public string[] sequence5_2 = { 
        "Unfortunately for these Jewish families, less than 8 months later, their homes were sold again. This time to the Christian nobleman Iñigo de Camudio for 100,000 silver pieces.",
        "Iñigo’s purchase of these homes was part of a secret arrangement that would facilitate the entry of a combative political leader, the Count of Béjar, into the city. ",
        "The count was the hidden party who had provided the silver to buy <i>La Mota</i>.",
        "In 1426, Count Pedro de Estúñiga forced all of the Jewish clans from their residences. ",
        "Their ejection from these domiciles was the product of a brewing competition.",
        "It was an emerging regional war between the Carvajal-Santa María family of knights and churchmen ...and the interloping and powerful Estúñiga family. ",
        "Curiously, both families were descended from the intermarriage of Christian noblemen and Jewish elites. They were <i>conversos</i>."
	};
	public string[] sequence6_0 = { 
        "The machinations of Pedro de Estúñiga were unknown to all the parties now participating in these property transfers. ",
        "We are now back in the vicinity of the synagogue. Yuce Abencur, who was from a family of butchers, welcomed Fernándo de la Mota to enter his home. They too exchanged the customary handshake. "
                                  };
	public string[] sequence7_0 = { 
        "Many other homes in La Mota would still have to be transferred that day -- including the houses of Çag Pardo, a silversmith." };
	public string[] sequence8_0 = { 
        "As well as those of Hayn Daza and Symuel Daza, who were from a Jewish family of shoemakers.",
        "After the mid-1420s, the Estúñigas would quickly consolidate their land holdings in this section of the Jewish quarter. <i>La Mota</i> was no longer a Jewish neighborhood."
                                  };
	public string[] sequence9_0 = { 
        "Unsettled by the forceful removal of the Jews of <i>La Mota</i> and their own eroding power base, local Christian leaders contested the count’s growing control. ",
        "In 1431, a tumultuous regional war would find Rabbi Abraham de Loya imprisoned and placed in shackles. ",
        "Commoners of all faith groups were intimidated with the erection of hanging gallows in nearby towns.",
        "Two Placentino Jews -- Fartalo and his wife -- were murdered and their bodies pulled from a nearby river.",
        "This was the other side of Spain’s <i>convivencia</i> -- violence."
	};
	public string[] sequence10_0 = { 
        "Standing before us now is Count Pedro de Estúñiga.",
        "In December of 1441, King Juan II gave Plasencia and “all rights over the city” to Pedro in return for his political loyalty.",
        "The count moved quickly to build his Palace of the Mirabeles -- right in the heart of <i>La Mota</i>.",
        "Not only were Christian nobles to receive Pedro as their lord -- but the Jewish community became the property of the count. ",
        "But there were limits to his power. The city enjoyed the benefits of a charter (<i>fuero</i>), which prevented Pedro from disbanding the city council. For the next 50 years, the count and the city’s nobles would live a contentious Christian coexistence of their own."
	};
	public string[] sequence11_0 = { 
		"During the remainder of the 1440s, the Carvajal-Santa María family confederation used a combination of personal wealth, church authority, and church statutes to block Pedro de Estúñiga’s attempt to solidify control over the region and its inhabitants. These actions revealed that these conversos (families of mixed Jewish-Christian pedigree) were very protective of their Jewish neighbors. ", 
		"For example, three months after the count assumed his new title, the Carvajal-Santa María family used their countervailing power in the cathedral to manipulate real estate transactions and to slow the Estúñigas’ consolidation of control over the city’s Jewish quarter. "
	};
    public string[] sequence11_0_0 = { 
        "<i>La Mota</i> was lost as a predominantly Jewish neighborhood. The palace would expand in size and the synagogue would be demolished.",
                                     "In a donation recorded on July 22, 1477, the Estúñiga family gave the Jewish neighborhood of <i>La Mota</i> and the synagogue to the Dominicans to construct a monastery."};
	public string[] sequence11_0_1 = { 
                                         "By virtue of his “seigniorial privilege and ownership and possession of the Jews and the Jewish community”, Alvaro gave the synagogue and “those houses that belonged to Rabbi Abraham” and others structures to the religious order charged with protecting the Christian faith from heretics."
};
	public string[] sequence12_0 = { 
		"But, <i>La Mota</i> was lost as a predominantly Jewish zone. It would now become the central locus of power for the count and his family until the end of the 15th century. The Palace of Mirabeles would expand in size and the synagogue would be demolished so that a monastery could be erected.", 
		"In a donation recorded in the Registro del Sello de Corte of the Kingdom of Castile and Leon, on July 22, 1477, Alvaro de Estúñiga gave the Jewish neighborhood of <i>La Mota</i> and the synagogue to the Dominicans to construct a monastery.", 
		"By virtue of his “seigniorial privilege and ownership and possession of the Jews and the Jewish community”, Alvaro gave the synagogue, “those houses that belonged to Rabbi Abraham”, and others structures, to the religious order charged with protecting the Christian faith from heretics."
	};
	public string[] sequence13_0 = { 
        "The response of the Carvajal-Santa María family confederation was remarkable as it demonstrated that not all conversos were committed to the elimination of the Jewish community. ", 
    "Rather these clans stood steadfast and ready to integrate the displaced Jews and synagogue in their immediate neighborhood in Plasencia. Within eight days of the count’s actions, on July 30, 1477, Rodrigo de Carvajal sold “forever after” – rather than just leasing – a large complex of his homes on Calle de Zapateria to Saul Daza and Yuda Fidauque.", 
    "Just as had been done in the <i>La Mota</i> over sixty years earlier but in a reversal of roles, this time Jews took possession of Christian homes. Saul and Yuda did so by “entering into them and walking through them, and to signal their possession of them, in the presence of everyone that was there, they closed the houses’ doors while they remained inside of them.”",
    "Proving the strength of their conviction that Plasencia’s Jewish community should have a proper synagogue, the Carvajal-Santa María family confederation sold multiple properties so that a new synagogue could be constructed on the Plaza de Don Marcos. \n\nReligious co-existence had not come to an end in Plasencia...but now it was entering its twilight."
};
	public string[] sequence13_0_0 = { 
        "Yet, <i>convivencia</i> was not dead in Plasencia.",
                                     "Eight days after Pedro’s spiritual attack on the Jewish community, the Carvajal-Santa Maria family of knights and churchmen proved the strength of their convictions."};
    public string[] sequence13_0_1 = { 
  "The Christian clans stood steadfast with their Jewish counterparts."};
	public string[] sequence13_0_2 = { 
   "These Christians, who were themselves descended from Jewish ancestries, sold large housing complexes to many Jewish families."};
	public string[] sequence13_0_3 = { 
   "More importantly, with these Christians’ blessing and support, the Jewish community built a new synagogue within the newly emergent enclave of Jewish and Christian homes on the Plaza de Don Marcos. "};
   
    
	public string[] sequence13_0_4 = {
    "By the end of 1488, religious coexistence had not come to an end in Plasencia...but it was entering its twilight."};

#endregion PLASENCIA

	public Dictionary<string, string[]> texts;

	void Start ()
	{
		#region PLASENCIATEXTADD
		texts = new Dictionary<string, string[]> ();
		texts.Add ("sequence1_0", 
            sequence1_0);
		texts.Add ("sequence1_1", 
            sequence1_1);
		texts.Add ("sequence1_2", 
            sequence1_2);
		texts.Add ("sequence2_0", 
            sequence2_0);
		texts.Add ("sequence3_0", 
            sequence3_0);
		texts.Add ("sequence4_0", 
            sequence4_0);
		texts.Add ("sequence5_1", 
            sequence5_1);
		texts.Add ("sequence5_2", 
            sequence5_2);
		texts.Add ("sequence6_0", 
            sequence6_0);
		texts.Add ("sequence7_0", 
            sequence7_0);
		texts.Add ("sequence8_0", 
            sequence8_0);
		texts.Add ("sequence9_0", 
            sequence9_0);
		texts.Add ("sequence10_0", 
            sequence10_0);
		texts.Add ("sequence11_0", 
            sequence11_0);
		texts.Add ("sequence12_0", 
            sequence12_0);
		texts.Add ("sequence13_0", 
            sequence13_0);
		texts.Add ("sequence1_0_0",
                    sequence1_0_0);
		texts.Add ("sequence1_0_1",
                    sequence1_0_1);
		texts.Add ("sequence1_0_2",
                    sequence1_0_2);
		texts.Add ("sequence1_0_3",
                            sequence1_0_3);
		texts.Add ("sequence1_0_4",
                            sequence1_0_4);
		texts.Add ("sequence1_0_5",
                            sequence1_0_5);
		texts.Add ("sequence11_0_0", sequence11_0_0);
		texts.Add ("sequence11_0_1", sequence11_0_1);

		texts.Add ("sequence13_0_0", sequence13_0_0); 
		texts.Add ("sequence13_0_1", sequence13_0_1);
		texts.Add ("sequence13_0_2", sequence13_0_2);
		texts.Add ("sequence13_0_3", sequence13_0_3);
		texts.Add ("sequence13_0_4", sequence13_0_4);

		#endregion




		texts.Add ("sequence1", sequence1);
		texts.Add ("sequence2", sequence2);
		texts.Add ("sequence3", sequence3);
		texts.Add ("sequence4", sequence4);
		texts.Add ("sequence5", sequence5);
		texts.Add ("sequence6", sequence6);
		texts.Add ("sequence7", sequence7);
		texts.Add ("sequence8", sequence8);
		texts.Add ("sequence9", sequence9);
		texts.Add ("sequence10", sequence10);
		texts.Add ("sequence11", sequence11);
		texts.Add ("sequence12", sequence12);
		texts.Add ("sequence13", sequence13);
		texts.Add ("sequence14", sequence14);
		texts.Add ("sequence15", sequence15);
        texts.Add("sequence16", sequence16);


	}

	public Dictionary<string, string[]> getTextDictionary ()
	{ 
		return this.texts;
	}
}
