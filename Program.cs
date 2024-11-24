class Solver {
    public static List<T> flatten<T>(List<List<T>> list){
        List<T> result = new List<T>();

        foreach (List<T> nested in list){
            foreach (T item in nested){
                result.Add(item);
            }
        }

        return result;
    }

    public static List<List<T>> getPermutations<T>(List<T> values, uint length) {
        List<List<T>> result = new List<List<T>>();

        if (length == 1){
            List<T> temp = new List<T>();
            temp.Add(values[0]);
            result.Add(temp);
            return result;
        }

        for (int i = 0; i < values.Count; i++){
            List<T> temp = new List<T>();

            for (int j = 0; j < values.Count; j++){
                if (j!= i) {
                    temp.Add(values[j]);
                }
            }

            List<List<T>> perms = getPermutations(temp, length - 1);

            foreach (List<T> perm in perms) {
                perm.Insert(0, values[i]);
                result.Add(perm);
            }
        }

        return result;
    }

    public static void solveNumberPuzzle(string puzzle){
        Dictionary<char, int> charSet = new Dictionary<char, int>();

        foreach (char c in puzzle){
            if (char.IsAsciiLetter(c)){
                if (!charSet.ContainsKey(c))
                    charSet.Add(c, -1);
            }
        }

        foreach (var (c,i) in charSet){
            Console.WriteLine(c);
            Console.WriteLine(i);
        }

        uint permutationLength = Convert.ToUInt32(charSet.Count);
        List<int> values = new List<int>();

        for (int i = 0; i < 10; i++){
            values.Add(i);
        }

        foreach (var item in values){
            Console.WriteLine(item);
        }

        List<List<int>> permutations = getPermutations<int>(values, permutationLength);

        /*foreach (var perm in permutations) {*/
        /*    Console.Write("List - ");*/
        /**/
        /*    foreach (var item in perm){*/
        /*        Console.Write(item);*/
        /*        Console.Write(" ");*/
        /*    }*/
        /**/
        /*    Console.WriteLine();*/
        /*}*/

        foreach (var perm in permutations){
            int i = 0;

            foreach (var c in charSet.Keys){
                charSet[c] = perm[i];
                i++;
            }

            for (int j = 0; j < puzzle.Length; j++){

            }
        }

    }

}

class Program {


    public static void Main(string[] args){
        /*Console.WriteLine("Hi Mom!");*/
        Solver.solveNumberPuzzle("odin+odin=mnogo");
    }
}
