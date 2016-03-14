using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ZOO;

namespace KontrolniTesty
{
	[TestClass]
	public class KontrolniTesty
	{
		private Type cAntilopa = Type.GetType("ZOO.Antilopa, ZOO");
		private Type cLev = Type.GetType("ZOO.Lev, ZOO");
		private Type cJidlo = Type.GetType("ZOO.Jidlo, ZOO");
		private Type cZvire = Type.GetType("ZOO.Zvire, ZOO");
		private static int nZvirat;
		[TestMethod]
		public void T01_TridaLev()
		{
			Assert.IsNotNull(cLev, "Nenalezena trida Lev!");
		}
		[TestMethod]
		public void T02_TridaAntilopa()
		{
			Assert.IsNotNull(cAntilopa, "Nenalezena trida Antilopa!");
		}
		[TestMethod]
		public void T03_TridaJidlo()
		{
			Assert.IsNotNull(cJidlo, "Nenalezena trida Jidlo!");
			Assert.IsTrue(
				cJidlo.IsAbstract,
				"new Jidlo() nesmi byt mozne pouzit"
			);
		}
		private object vytvorAlika()
		{
			nZvirat++;
			return Activator.CreateInstance(
				cLev,
				new object[] { (object)"Alik", (object)2 },
				new object[] { }
			);
		}
		[TestMethod]
		public void T04_LevKonstruktor()
		{
			vytvorAlika();
		}
		[TestMethod]
		public void T05_LevToString()
		{
			Assert.AreEqual(
				"lev(Jmeno=\"Alik\",Hmotnost=2)",
				vytvorAlika().ToString()
			);
		}
		private object vytvorAntilopu()
		{
			nZvirat++;
			return Activator.CreateInstance(
				cAntilopa,
				new object[] { (object)"A1", (object)7 },
				new object[] { }
			);
		}
		[TestMethod]
		public void T06_AntilopaKonstruktor()
		{
			vytvorAntilopu();
		}
		[TestMethod]
		public void T07_AntilopaToString()
		{
			Assert.AreEqual(
				"antilopa(Jmeno=\"A1\",Hmotnost=7)",
				vytvorAntilopu().ToString()
			);
		}
		[TestMethod]
		public void T08_TridaZvire()
		{
			Assert.IsNotNull(cZvire, "Nenalezena trida zvire!");
			Assert.IsTrue(
				cZvire.IsAbstract,
				"Je zakazano vytvaret zvirata bez specifikovaneho druhu."
				 + " Chybi klicove slovo abstract."
			);
		}
		[TestMethod]
		public void T09_OpakovanyKod()
		{
			var fields = BindingFlags.NonPublic | BindingFlags.Instance
				| BindingFlags.DeclaredOnly;
			string msg = "Trida Lev nebo Antilopa obsahuje neco nadbytecneho."
				+ " Nemelo to byt v rodicovske tride Zvire?";
			Assert.IsTrue(cLev.GetFields(fields).Length == 0, msg);
			Assert.IsTrue(cAntilopa.GetFields(fields).Length == 0, msg);
		}
		[TestMethod]
		public void T10_OpakovanyKod2()
		{
			string msg = "Neopakuje se nahodou podobny kod v metode ToString"
			+ " u tridy Lev a Antilopa? Je chybou mit podobny kod na vice mistech."
			+ " Veci shodne pro vsechna zvirata by meli byt implementovany"
			+ " v rodicovske tride Zvire."
			+ " Az cinnosti, ktere jsou pro kazde zvire ruzne,"
			+ " by meli byt implementovany v jednotlivych potomcich."
			+ " Zde se ruzni jen jednoslovne jmeno druhu."
			+ " Navod na reseni:"
			+ " do tridy Zvire pridejte protected abstract string druh();"
			+ " V potomcich lze tuto metodu implementovat nasledovne:"
			+ " protected override string druh() { return \"lev\"; }";
			Assert.IsTrue
				(cLev.GetMethod("ToString").DeclaringType == cZvire, msg);
			Assert.IsTrue
				(cAntilopa.GetMethod("ToString").DeclaringType == cZvire, msg);
		}

		private object rozmnozAlika()
		{
			object alik = vytvorAlika();
			nZvirat++;
			MethodInfo methodInfo = cLev.GetMethod("RozmnozSe");
			return methodInfo.Invoke(alik, new object[0]);
		}
		[TestMethod]
		public void T11_LevJuniorToString()
		{
			Assert.AreEqual(
				"lev(Jmeno=\"Alik junior\",Hmotnost=1)",
				rozmnozAlika().ToString()
			);
		}
		private object vytvorBetinu()
		{
			nZvirat++;
			return Activator.CreateInstance(
				cAntilopa,
				new object[] { (object)"Betina", (object)2 },
				new object[] { }
			);
		}
		private object rozmnozBetinu()
		{
			object betina = vytvorBetinu();
			nZvirat++;
			MethodInfo methodInfo = cAntilopa.GetMethod("RozmnozSe");
			return methodInfo.Invoke(betina, new object[0]);
		}
		[TestMethod]
		public void T12_AntilopaJuniorToString()
		{
			Assert.AreEqual(
				"antilopa(Jmeno=\"Betina junior\",Hmotnost=1)",
				rozmnozBetinu().ToString()
			);
		}
		[TestMethod]
		public void T13_PropertyZije()
		{
			var pZije = cZvire.GetProperty("Zije");
			Assert.IsNotNull(pZije,
				"Dalsi casti zadani je implementovat property Zije."
			);
			Assert.IsTrue(pZije.DeclaringType == cZvire,
				"Property Zije by mela byt deklarovana ve tride Zvire."
			);
			string message
				= "Prave narozena zvirata (lev, antilopa) by mela byt ziva";
			Assert.IsTrue((bool)pZije.GetValue(vytvorAlika()), message);
			Assert.IsTrue((bool)pZije.GetValue(vytvorBetinu()), message);
		}
		[TestMethod]
		public void T14_OpakovanyKod3()
		{
			string message
			= "Ani v konstruktorech by nemel byt opakujici se kod."
			+ " Cinnosti, ktere se opakuji pro vsechna zvirata,"
			+ " by mela byt ve tride Zvire."
			+ " Konstruktor lva muze vypadat nasledovne:"
			+ " public Lev(string jmeno, int hmotnost)"
			+ " : base(jmeno, hmotnost) {}";
			var constructor = cLev.GetConstructor(
				new Type[] { typeof(string), typeof(int) }
			);
			int delka = constructor.GetMethodBody().GetILAsByteArray().Length;
			Assert.IsTrue(delka <= 12, message);
			constructor = cAntilopa.GetConstructor(
				new Type[] { typeof(string), typeof(int) }
			);
			delka = constructor.GetMethodBody().GetILAsByteArray().Length;
			Assert.IsTrue(delka <= 12, message);
		}
		private void zkontrolujKonzoli(Action a, string expectedOutput)
		{
			var ms = new System.IO.MemoryStream();
			var sw = new StreamWriter(ms);
			Console.SetOut(sw);
			a();
			sw.Flush();
			ms.Seek(0, SeekOrigin.Begin);
			var tr = new StreamReader(ms);
			string output = tr.ReadToEnd();
			Assert.AreEqual(expectedOutput, output);
		}
		[TestMethod]
		public void T15_Snez()
		{
			var mSnez = cZvire.GetMethod("Snez");
			Assert.IsNotNull(mSnez, "Zvire by melo mit metodu Snez");
			var alik = vytvorAlika();
			var betina = vytvorBetinu();
			zkontrolujKonzoli(
				() => { mSnez.Invoke(alik, new object[] { betina }); },
				"Antilopa Betina se pokousi utect."
				+ Environment.NewLine
				+ "Umrtni oznameni: antilopa(Jmeno=\"Betina\",Hmotnost=2)"
				+ Environment.NewLine
			);
			Assert.IsTrue(
				alik.ToString() == "lev(Jmeno=\"Alik\",Hmotnost=4)",
				"Pokud zvire neco sni,"
				+ " tak by se jeho hmotnost mela navysit o hmotnost jidla"
			);
			Assert.IsTrue(
				betina.ToString() == "antilopa(Jmeno=\"Betina\",Hmotnost=0)",
				"Hmotnost ostatku snezeneho zvirete by mela byt 0."
			);
			//Console.SetOut(new System.IO.MemoryStream(
			var pZije = cZvire.GetProperty("Zije");
			Assert.IsTrue(
				((bool)pZije.GetValue(betina)) == false,
				"Pokud je snezeno jidlo, tak musi umrit."
			);
			zkontrolujKonzoli(
				() => { mSnez.Invoke(alik, new object[] { alik }); },
				"Umrtni oznameni: lev(Jmeno=\"Alik\",Hmotnost=8)"
				 + Environment.NewLine
			);
		}
		[TestMethod]
		public void T16_ZakazOzivovani()
		{
			var pZije = cZvire.GetProperty("Zije");
			Assert.IsTrue(pZije.SetMethod.IsPrivate,
				"Property Zije by mela byt verejne jen pro cteni."
			);
		}
		[TestMethod]
		public void T17_MetodaUmri()
		{
			var mUmri = cZvire.GetMethod("Umri");
			Assert.IsNotNull(mUmri, "Zvire by melo mit metodu Umri.");
			Assert.IsTrue(mUmri.DeclaringType == cZvire,
				"Metoda umri by mela byt ve tride zvire"
			);
			zkontrolujKonzoli(
				() => { mUmri.Invoke(vytvorAlika(), new object[0]); },
				"Umrtni oznameni: lev(Jmeno=\"Alik\",Hmotnost=2)"
				+ Environment.NewLine
			);
			zkontrolujKonzoli(
				() => { mUmri.Invoke(vytvorBetinu(), new object[0]); },
				"Umrtni oznameni: antilopa(Jmeno=\"Betina\",Hmotnost=2)"
				+ Environment.NewLine
			);
		}
		[TestMethod]
		public void T18_PocetZvirat()
		{
			var m = cZvire.GetMethod("VytvorenoZvirat");
			Assert.IsNotNull(m, "Chybi metoda VytvorenoZvirat,"
				+ " krera vrati pocet zvirat vytvorenych v simulaci."
			);
			Assert.AreEqual(nZvirat, ((int)m.Invoke(null, new object[0])));
		}
	}
}
