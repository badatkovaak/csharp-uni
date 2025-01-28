public class Utils
{
    public static T Max<T>(params T[] args)
        where T : IComparable<T>
    {
        if (args.Length == 0)
        {
            throw new System.Exception();
        }

        T result = args[0];

        foreach (T t in args)
        {
            if (t.CompareTo(result) > 0)
            {
                result = t;
            }
        }

        return result;
    }

    public static T Min<T>(params T[] args)
        where T : IComparable<T>
    {
        if (args.Length == 0)
        {
            throw new System.Exception();
        }

        T result = args[0];

        foreach (T t in args)
        {
            if (t.CompareTo(result) < 0)
            {
                result = t;
            }
        }

        return result;
    }

    public static List<T> Concat<T>(params List<T>[] args)
    {
        int len = 0;
        foreach (List<T> l in args)
            len += l.Count;

        List<T> res = new List<T>(len);

        foreach (List<T> l in args)
        foreach (T item in l)
            res.Add(item);

        return res;
    }

    public static List<T> CollectToList<T>(params T[] args)
    {
        List<T> res = new List<T>();

        foreach (T item in args)
        {
            res.Add(item);
        }

        return res;
    }
}
