using System;
using System.Collections.Generic;

using UnityEngine;

namespace POP
{
	/// <summary>
	/// 2-element tuple, with the first element being of type S, the second of type T.
	/// </summary>
	[Serializable]
	public class Tuple<S, T> : IEquatable<Tuple<S,T>>{

		[SerializeField]
		private S element1;

		[SerializeField]
		private T element2;

		public Tuple(S element1, T element2)
		{
			this.element1 = element1;
			this.element2 = element2;
		}

		public override int GetHashCode()
		{
			return ((element1.GetHashCode() << 5)+ element1.GetHashCode()) ^ element2.GetHashCode();
			// ((h1 << 5) + h1) ^ h2)
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			return Equals((Tuple<S,T>)obj);
		}

		public bool Equals(Tuple<S,T> other)
		{
			return other.Element1.Equals(element1) && other.Element2.Equals(element2);
		}

		public override string ToString ()
		{
			return string.Format ("[Tuple: Element1={0}, Element2={1}]", Element1, Element2);
		}
		public Tuple(KeyValuePair<S, T> pair) : this(pair.Key, pair.Value) { }

		public S Element1 
		{
			get { return element1; }
			set { element1 = value; }
		}

		public T Element2 
		{
			get { return element2; }
			set { element2 = value; }
		}
		
	}

	/// <summary>
	/// 3-element tuple, with the first element being of type S, the second of type T and the third element of type R.
	/// </summary>
	[Serializable]
	public class Tuple<S, T, R> : IEquatable<Tuple<S,T, R>>{

		private S element1;

		private T element2;

		private R element3;
		
		public Tuple(S element1, T element2, R element3)
		{
			this.element1 = element1;
			this.element2 = element2;
			this.element3 = element3;
		}
		
		public override int GetHashCode()
		{
			return ((element1.GetHashCode() << 5)+ element1.GetHashCode()) ^ element2.GetHashCode() ^ element3.GetHashCode();
			// ((h1 << 5) + h1) ^ h2)
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			return Equals((Tuple<S,T,R>)obj);
		}
		
		public bool Equals(Tuple<S,T,R> other)
		{
			if (other.Element2 == null)
				return other.Element1.Equals(element1) &&  other.Element3.Equals(element3);
			return other.Element1.Equals(element1) && other.Element2.Equals(element2) && other.Element2.Equals(element3);
		}
		
		public override string ToString ()
		{
			return string.Format ("[Tuple: Element1={0}, Element2={1}, Element3={2}]", Element1, Element2, Element3);
		}
		
		public S Element1 
		{
			get { return element1; }
			set { element1 = value; }
		}
		
		public T Element2 
		{
			get { return element2; }
			set { element2 = value; }
		}
		
		public R Element3
		{
			get { return element3; }
			set { element3 = value; }
		}
		
	}
}