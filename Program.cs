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

            foreach (T t in values){
                List<T> temp = new List<T>();
                temp.Add(t);
                result.Add(temp);
            }

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
        /*int i = 0;*/
        /*while(i < puzzle.Length){*/
        /*    if (puzzle)*/
        /*}*/

        string[] parts = puzzle.Split('+');
        string[] temp = parts[parts.Length - 1].Split('=');
        string lhs = temp[1] ;
        parts[parts.Length - 1] = temp[0];

        /*foreach (var c in parts){*/
        /*    Console.WriteLine(c);*/
        /*}*/
        /**/
        /*Console.WriteLine(lhs);*/


        Dictionary<char, int> charSet = new Dictionary<char, int>();

        foreach (char c in puzzle){
            if (char.IsAsciiLetter(c)){
                if (!charSet.ContainsKey(c))
                    charSet.Add(c, -1);
            }
        }

        /*foreach (var (c,i) in charSet){*/
        /*    Console.WriteLine(c);*/
        /*    Console.WriteLine(i);*/
        /*}*/

        uint permutationLength = Convert.ToUInt32(charSet.Count);
        List<int> values = new List<int>();

        for (int i = 0; i < 10; i++){
            values.Add(i);
        }

        /*foreach (var item in values){*/
        /*    Console.WriteLine(item);*/
        /*}*/

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
        /**/
        /*Console.WriteLine(permutations.Count);*/


        foreach (var perm in permutations){
            int i = 0;

            foreach (char c in charSet.Keys){
                charSet[c] = perm[i];
                i++;
            }

            /*foreach (char c in charSet.Keys){*/
            /*    Console.WriteLine($"{c} - {charSet[c]}");*/
            /*}*/

            string[] new_parts = new string[parts.Length];
            string new_lhs = lhs;

            for(int j = 0; j < parts.Length; j++){
                new_parts[j] = parts[j];

                foreach (char c in charSet.Keys) {
                    new_parts[j] = new_parts[j].Replace(c, Convert.ToChar(48 + charSet[c]));
                    /*Console.WriteLine($"{new_parts[j]}  {c}  {Convert.ToChar(48 + charSet[c])}");*/
                }
            }

            foreach (char c in charSet.Keys){
                new_lhs = new_lhs.Replace(c, Convert.ToChar(48 + charSet[c]));
                /*Console.WriteLine(new_lhs);*/
            }



            int result = 0;

            foreach (string part in new_parts){
                result += Convert.ToInt32(part);
            }

            if (result == Convert.ToInt32(new_lhs)){
                Console.WriteLine("Success !");

                foreach (string part in new_parts) {
                    Console.Write($"{part}");
                }

                Console.WriteLine($"{new_lhs}");
            }

        }

    }

}

class Program {
    public static void Main(string[] args){
        Solver.solveNumberPuzzle("luba + lubit = arbvzj");
        /*Solver.solveNumberPuzzle("kto + kot = tok");*/
        /*Solver.solveNumberPuzzle("lisa + volk = zveri");*/
        /*Solver.solveNumberPuzzle("oxoxo + axaxa = axaxax");*/
        /*Solver.solveNumberPuzzle("abcde + sped = etebss");*/
        /*Solver.solveNumberPuzzle("sinus + sinus + kosinus = tangens");*/
        /*Solver.solveNumberPuzzle("bir + bir + bir + bir = dord");*/
        /*Solver.solveNumberPuzzle("aad + dab = bgg");*/
        /*Solver.solveNumberPuzzle("uran + uran = nauka");*/
        /*Solver.solveNumberPuzzle("odin + odin = mnogo");*/
    }
}
