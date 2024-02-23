// dnSpy decompiler from Assembly-CSharp.dll class: ListG`1
using System;
using System.Collections;
using System.Collections.Generic;

public class ListG<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable where T : ListNodeG<T>
{
	public int Count
	{
		get
		{
			return this.size;
		}
	}

	public bool IsSynchronized
	{
		get
		{
			return false;
		}
	}

	public object SyncRoot
	{
		get
		{
			return null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	public T Head
	{
		get
		{
			return this.head;
		}
	}

	public T Tail
	{
		get
		{
			return this.tail;
		}
	}

	public void Add(T item)
	{
		if (null == this.head)
		{
			this.tail = item;
			this.head = item;
			item.previous = (item.next = (T)((object)null));
		}
		else
		{
			this.tail.next = item;
			item.previous = this.tail;
			item.next = (T)((object)null);
			this.tail = item;
		}
		this.size++;
	}

	public void Clear()
	{
		T next = this.head;
		while (next != null)
		{
			T t = next;
			next = next.next;
			t.previous = (t.next = (T)((object)null));
		}
		this.head = (this.tail = (T)((object)null));
		this.size = 0;
	}

	public bool Contains(T item)
	{
		T next = this.head;
		while (next != null)
		{
			if (next == item)
			{
				return true;
			}
			next = next.next;
		}
		return false;
	}

	public void CopyTo(Array array, int index)
	{
		T next = this.head;
		while (next != null)
		{
			array.SetValue(next, index++);
			next = next.next;
		}
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		T next = this.head;
		while (next != null)
		{
			array[arrayIndex++] = next;
			next = next.next;
		}
	}

	public bool Remove(T item)
	{
		if (item == this.head)
		{
			if (item == this.tail)
			{
				this.head = (this.tail = (T)((object)null));
			}
			else
			{
				this.head = this.head.next;
				this.head.previous = (T)((object)null);
			}
		}
		else if (item == this.tail)
		{
			this.tail = this.tail.previous;
			this.tail.next = (T)((object)null);
		}
		else
		{
			if (null == item.previous && null == item.next)
			{
				return false;
			}
			item.previous.next = item.next;
			item.next.previous = item.previous;
		}
		item.previous = (item.next = (T)((object)null));
		this.size--;
		return true;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new ListG<T>.Enumerator
		{
			list = this,
			node = (T)((object)null)
		};
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public void ForEach(Action<T> function)
	{
		T next = this.head;
		while (this.head != null)
		{
			function(next);
			next = next.next;
		}
	}

	protected T head;

	protected T tail;

	protected int size;

	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		public T Current
		{
			get
			{
				return this.node;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this.node;
			}
		}

		public bool MoveNext()
		{
			if (null == this.node)
			{
				this.node = this.list.head;
				return this.node != null;
			}
			this.node = this.node.next;
			return this.node != null;
		}

		public void Reset()
		{
			this.node = (T)((object)null);
		}

		public void Dispose()
		{
			this.list = null;
			this.node = (T)((object)null);
		}

		internal ListG<T> list;

		internal T node;
	}
}
