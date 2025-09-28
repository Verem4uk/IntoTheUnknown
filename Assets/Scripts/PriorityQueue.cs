using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> Elements = new();

    public int Count => Elements.Count;

    public void Enqueue(T item, int priority)
    {
        Elements.Add((item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 1; i < Elements.Count; i++)
        {
            if (Elements[i].priority < Elements[bestIndex].priority)
            {
                bestIndex = i;
            }
        }

        T bestItem = Elements[bestIndex].item;
        Elements.RemoveAt(bestIndex);
        return bestItem;
    }
}
