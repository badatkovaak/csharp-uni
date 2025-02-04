using System;

public abstract class Animal {
    public abstract void make_noise();
    public abstract void what_do_i_eat();
    public abstract void get_description();
}

public class Cat: Animal {
    public Cat() {}

    public override void make_noise() {
        Console.WriteLine("мяу, мяу");
    }

    public override void what_do_i_eat() {
        Console.WriteLine("я ем мясо");
    }

    public override  void get_description() {
        Console.WriteLine("я кошка");
    } 
}


public class Dog: Animal {
    public Dog() {}

    public override void make_noise() {
        Console.WriteLine("гав, гав");
    }

    public override void what_do_i_eat() {
        Console.WriteLine("я ем мясо");
    }

    public override  void get_description() {
        Console.WriteLine("я собака");
    } 
}

public class Program {
    public static void Main(string[] args) {
        Cat c = new Cat();
        Dog d = new Dog();
        
        c.make_noise();
        d.what_do_i_eat();
        c.get_description();
        d.make_noise();
        d.what_do_i_eat();
        d.get_description();
    }
} 
