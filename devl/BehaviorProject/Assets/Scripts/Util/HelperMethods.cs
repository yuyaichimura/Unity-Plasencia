using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;

namespace POP
{

	public static class HelperMethods
	{
		/// <summary>
		/// Produces the Cartesian Product of an unknown number of lists
		/// </summary>
		public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
		{
			IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
			return sequences.Aggregate(
				emptyProduct,
				(accumulator, sequence) =>
				from accseq in accumulator
				from item in sequence
				select accseq.Concat(new[] {item}));
		}
	}
}

