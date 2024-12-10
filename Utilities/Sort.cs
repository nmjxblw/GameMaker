using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Sort
	{
		static Sort() { }
		public static T[] QuickSort<T>(T[] arr) where T : IComparable<T>
		{
			if (arr.Length <= 1)
				return arr;
			if (arr.Length < 1000)
			{
				T innerPivot = arr[arr.Length / 2];
				T[] ileft = arr.Where(x => x.CompareTo(innerPivot) < 0).ToArray();
				T[] iequal = arr.Where(x => x.CompareTo(innerPivot) == 0).ToArray();
				T[] iright = arr.Where(x => x.CompareTo(innerPivot) > 0).ToArray();
				return QuickSort(ileft).Concat(iequal).Concat(QuickSort(iright)).ToArray();
			}
			T pivot = arr[arr.Length / 2];
			T[] left = arr.Where(x => x.CompareTo(pivot) < 0).ToArray();
			T[] equal = arr.Where(x => x.CompareTo(pivot) == 0).ToArray();
			T[] right = arr.Where(x => x.CompareTo(pivot) > 0).ToArray();

			Task<T[]> leftTask = Task.Run(() => QuickSort(left));
			Task<T[]> rightTask = Task.Run(() => QuickSort(right));
			Task.WaitAll(leftTask, rightTask);
			return leftTask.Result.Concat(equal).Concat(rightTask.Result).ToArray();
		}
	}
}
