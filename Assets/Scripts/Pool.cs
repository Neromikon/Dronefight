using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
class Pool<Type> : IEnumerable<Pool<Type>.Enumerator>
{
	public struct Enumerator : IEnumerator<Type>
	{
		private readonly Pool<Type> pool;
		private int index;
		private int iterations;

		public Enumerator(Pool<Type> pool)
		{
			this.pool = pool;
			index = pool.firstElementIndex;
			iterations = 0;
		}

		public void Dispose()
		{
			//Dispose(true);
			GC.SuppressFinalize(this);
		}

		public Current
		{
			get { return pool[index].data; }
		}

		//public  Current
		//{
		//	get
		//	{
		//		if (this.pool == null || this.index == 0)
		//			throw new InvalidOperationException();

		//		return this.pool[this.index - 1];
		//	}
		//}

		public bool MoveNext()
		{
			index++;
			iterations++;

			if(iterations > Count) { return false; }

			while(!pool.data[index].isUsed) { index++; }

			return true;
		}

		public void Reset()
		{
			this.index = 0;
		}
	}

	private class Element
	{
		public bool isUsed;
		public Type data;
	}

	private List<Element> data;

	public int Capacity { get { return data.Capacity; } }
	public int Count { get; private set; }
	public int Space { get { return data.Capacity - Count; } }
	public bool Empty { get { return Count == 0; } }

	public int firstElementIndex { get; private set; };

	private int addSearchStartIndex = 0;
	//private int lastElementIndex = 0;

	public Pool(int capacity)
	{
		data = new List<Element>(capacity);
	}

	public Type Add()
	{
		for (int i = addSearchStartIndex; i < data.Capacity; i++)
		{
			if (data[i].isUsed) { continue; }

			addSearchStartIndex = i + 1;

			if (i < firstElementIndex) { firstElementIndex = i; }
			//if (i > lastElementIndex) { lastElementIndex = i; }

			Count++;

			data[i].isUsed = true;

			return data[i].data;
		}

		return default(Type);
	}

	public void Add(out Type member, out int index)
	{
		for (int i = addSearchStartIndex; i < data.Capacity; i++)
		{
			if (data[i].isUsed) { continue; }

			addSearchStartIndex = i + 1;

			if (i < firstElementIndex) { firstElementIndex = i; }
			//if (i > lastElementIndex) { lastElementIndex = i; }

			Count++;

			data[i].isUsed = true;

			member = data[i].data;
			index = i;
		}

		member = default(Type);
		index = -1;
	}

	public void Remove(Type member)
	{
		RemoveAt(data.FindIndex(firstElementIndex, f => { return f.data.Equals(member); }));
	}

	public void RemoveAt(int index)
	{
		data[index].isUsed = false;
		Count--;

		if (index == firstElementIndex)
		{
			for (int i = index + 1; i <= data.Capacity; i++)
			{
				if (data[i].isUsed) { firstElementIndex = i; }
			}
		}

		addSearchStartIndex = index;
	}

	public Type this[int index]
	{
		get { return data[index].data; }
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}
}

*/