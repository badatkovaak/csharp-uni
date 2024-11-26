class Solver {
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

        string[] parts = puzzle.Split('+');
        string[] temp = parts[parts.Length - 1].Split('=');
        string lhs = temp[1] ;
        parts[parts.Length - 1] = temp[0];

        Dictionary<char, int> charSet = new Dictionary<char, int>();

        foreach (char c in puzzle){
            if (char.IsAsciiLetter(c)){
                if (!charSet.ContainsKey(c))
                    charSet.Add(c, -1);
            }
        }

        uint permutationLength = Convert.ToUInt32(charSet.Count);
        List<int> values = new List<int>();

        for (int i = 0; i < 10; i++){
            values.Add(i);
        }

        List<List<int>> permutations = getPermutations<int>(values, permutationLength);

        foreach (var perm in permutations){
            int i = 0;

            foreach (char c in charSet.Keys){
                charSet[c] = perm[i];
                i++;
            }

            string[] new_parts = new string[parts.Length];
            string new_lhs = lhs;

            for(int j = 0; j < parts.Length; j++){
                new_parts[j] = parts[j];

                foreach (char c in charSet.Keys) {
                    new_parts[j] = new_parts[j].Replace(c, Convert.ToChar(48 + charSet[c]));
                }
            }

            foreach (char c in charSet.Keys){
                new_lhs = new_lhs.Replace(c, Convert.ToChar(48 + charSet[c]));
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
