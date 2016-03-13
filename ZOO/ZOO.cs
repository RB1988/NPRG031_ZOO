using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace ZOO
{
	// TODO
	// Sem doplnte implementaci dalsich trid, tak aby se kod nize zkompiloval.

	class Granule : Jidlo
	{ // O granulich se zadani nezminuje.
		// Jen chci, aby program umoznoval aby jidlo bylo neco jineho nez jen zvirata.
		// Granule se od zvirete lisi napriklad v tom ze nerozlisujeme zda jsou zive nebo mrtve.
		public Granule(int hmotnost) : base(hmotnost) { }
	}
	class ZOO
	{
		static void Main(string[] args)
		{
			/* new Zvire(); // Tento prikaz by nemelo byt mozne prelozit. */
			/* new Jidlo(); // Tento prikaz by nemelo byt mozne prelozit. */
			var lev1 = new Lev("Alik", 2);
			var lev2 = lev1.RozmnozSe();
			Console.WriteLine(lev1);
			// lev(Jmeno="Alik",Hmotnost=2)
			Console.WriteLine(lev2);
			// lev(Jmeno="Alik junior",Hmotnost=1)

			var antilopa1 = new Antilopa("Betina", 2);
			var antilopa2 = antilopa1.RozmnozSe();
			Console.WriteLine(antilopa1);
			// antilopa(Jmeno="Betina",Hmotnost=2)
			Console.WriteLine(antilopa2);
			// antilopa(Jmeno="Betina junior",Hmotnost=1)

			Console.WriteLine(antilopa1.ToString() + (antilopa1.Zije ? " zije!" : " je mrtva."));
			// antilopa(Jmeno="Betina",Hmotnost=2) zije!
			lev1.Snez(antilopa1);
			// Antilopa Betina se pokousi utect.
			// Umrtni oznameni: antilopa(Jmeno="Betina",Hmotnost=2)
			Console.WriteLine(antilopa1.ToString() + (antilopa1.Zije ? " zije" : " je mrtva."));
			// antilopa(Jmeno="Betina",Hmotnost=0) je mrtva
			Console.WriteLine("Hmotnost lva se zvysila o hmotnost antilopy");
			// Hmotnost lva se zvysila o hmotnost antilopy
			Console.WriteLine(lev1);
			// lev(Jmeno="Alik",Hmotnost=4)

			/* antilopa1.Zije = true; // tento prikaz by nemel jit prelozit */
			antilopa2.Umri();
			// Umrtni oznameni: antilopa(Jmeno="Betina junior",Hmotnost=1)

			lev2.Snez(lev1);
			// Umrtni oznameni: lev(Jmeno="Alik",Hmotnost=4)
			Console.WriteLine(Zvire.VytvorenoZvirat());
			// 4
			/*
				lev(Jmeno="Alik",Hmotnost=2)
				lev(Jmeno="Alik junior",Hmotnost=1)
				antilopa(Jmeno="Betina",Hmotnost=2)
				antilopa(Jmeno="Betina junior",Hmotnost=1)
				antilopa(Jmeno="Betina",Hmotnost=2) zije!
				Antilopa Betina se pokousi utect.
				Umrtni oznameni: antilopa(Jmeno="Betina",Hmotnost=2)
				antilopa(Jmeno="Betina",Hmotnost=0) je mrtva.
				Hmotnost lva se zvysila o hmotnost antilopy
				lev(Jmeno="Alik",Hmotnost=4)
				Umrtni oznameni: antilopa(Jmeno="Betina junior",Hmotnost=1)
				Umrtni oznameni: lev(Jmeno="Alik",Hmotnost=4)
				4
			*/
		}
	}
}
