using System;
using System.Collections.Generic;
using System.Collections;

class Primes : IEnumerable<int> {
    int start = 2;
    int end;

    List<int> primes;
    
    public Primes(int start, int end) {
        this.start = start;
        this.end = end;
        this.primes = GeneratePrimes(start , end);
    }

    static bool IsPrime(int x) {
        if (x%2 == 0)
            return false;

        for(int i = 3; i*i < x; i+=2)
            if (x % i == 0) 
                return false;

        return true;
    }

    static List<int> GeneratePrimes(int start, int end) {
        List<int> result = new List<int>();

        if (start == 2){
            result.Add(2);
            start++;
        }

        for(int i = start +  1 - start % 2; i < end; i+= 2)
            if (IsPrime(i))
                result.Add(i);

        return result;
    }

    public IEnumerator<int> GetEnumerator() {
        foreach(int i in primes)
            yield return i;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}
