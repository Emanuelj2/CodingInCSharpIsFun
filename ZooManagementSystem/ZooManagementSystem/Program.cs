// ============================================
// MAIN PROGRAM - Demonstrating all OOP concepts
// ============================================
class Program
{
    static void Main(string[] args)
    {

        // ========================================
        //    CREATING OBJECTS (INSTANTIATION)
        // ========================================
        Console.WriteLine("═══ 1. Creating Animals ═══");

        Lion simba = new Lion("Simba", 5, "African Lion", "Healthy", "Twice daily", "Golden", 6);
        Eagle freedom = new Eagle("Freedom", 3, "Bald Eagle", "Healthy", "Once daily", 2.3, true, 9);
        Snake kaa = new Snake("Kaa", 7, "Python", "Healthy", "Weekly", false, "Smooth", 4.5);

        Console.WriteLine("✓ Created: Simba (Lion), Freedom (Eagle), Kaa (Snake)\n");

        // ========================================
        //    DEMONSTRATING POLYMORPHISM
        // ========================================
        Console.WriteLine("═══ 2. Polymorphism - Different Animals, Same Method ═══");

        List<Animal> allAnimals = new List<Animal> { simba, freedom, kaa };

        foreach (var animal in allAnimals)
        {
            Console.Write($"{animal.Name} ({animal.GetType().Name}): ");
            animal.MakeSound();
        }

        // ========================================
        //    DEMONSTRATING INHERITANCE
        // ========================================
        Console.WriteLine("\n═══ 3. Three-Level Inheritance Hierarchy ═══");
        Console.WriteLine("Animal → Mammal → Lion");
        Console.WriteLine("Animal → Bird → Eagle");
        Console.WriteLine("Animal → Reptile → Snake\n");

        simba.DisplayInfo();
        Console.WriteLine($"Pride Size: {simba.PrideSize}");
        Console.WriteLine($"Fur Color: {simba.FurColor}");
        simba.Hunt();

        // ========================================
        //    DEMONSTRATING METHOD OVERRIDING
        // ========================================
        Console.WriteLine("\n═══ 4. Method Overriding - Eat() Method ═══");

        simba.Eat("raw meat");
        freedom.Eat("fresh fish");
        kaa.Eat("small rodent");

        // ========================================
        // 5. CREATING ZOOKEEPER AND ENCLOSURES
        // ========================================
        Console.WriteLine("\n═══ 5. Creating ZooKeeper and Enclosures ═══");

        ZooKeeper steve = new ZooKeeper("Steve Irwin", 101, "Exotic Animals");
        Console.WriteLine($"✓ ZooKeeper {steve.Name} (ID: {steve.EmployeeId}) assigned to {steve.AssignedSection}");

        Enclosure enclosure1 = new Enclosure(1, 500.0, "Savanna");
        Enclosure enclosure2 = new Enclosure(2, 300.0, "Mountain");
        Enclosure enclosure3 = new Enclosure(3, 200.0, "Tropical");

        // ========================================
        //    DEMONSTRATING COMPOSITION
        // ========================================
        Console.WriteLine("\n═══ 6. Composition - Enclosures Contain Animals ═══");

        enclosure1.AddAnimal(simba);
        enclosure2.AddAnimal(freedom);
        enclosure3.AddAnimal(kaa);

        // ========================================
        //    ZOOKEEPER INTERACTIONS
        // ========================================
        Console.WriteLine("\n═══ 7. ZooKeeper Feeding Animals (Polymorphism) ═══");

        steve.FeedAnimal(simba, "fresh zebra meat");
        steve.FeedAnimal(freedom, "salmon");
        steve.FeedAnimal(kaa, "live mouse");

        // ========================================
        //    HEALTH CHECK UPDATES
        // ========================================
        Console.WriteLine("\n═══ 8. Health Check - Updating Animal Properties ═══");

        steve.PerformHealthCheck(simba, "Excellent");
        steve.PerformHealthCheck(freedom, "Minor wing injury");

        // ========================================
        //    INTERFACE IMPLEMENTATION
        // ========================================
        Console.WriteLine("\n═══ 9. Interface (IFeedable) Implementation ═══");

        steve.PrepareFeedingSchedule(simba);
        steve.PrepareFeedingSchedule(freedom);
        steve.PrepareFeedingSchedule(kaa);

        // ========================================
        //     INTERFACE WITH POLYMORPHISM
        // ========================================
        Console.WriteLine("\n═══ 10. Treating Animals as IFeedable ═══");

        List<IFeedable> feedableAnimals = new List<IFeedable> { simba, freedom, kaa };

        foreach (var feedable in feedableAnimals)
        {
            Animal animal = feedable as Animal;
            Console.WriteLine($"{animal?.Name} needs {feedable.CalculateFoodAmount()} kg at {feedable.GetFeedingTime().ToString("h:mm tt")}");
        }

        // ========================================
        //     ENCLOSURE OPERATIONS
        // ========================================
        Console.WriteLine("\n═══ 11. Listing All Animals in Enclosures ═══");

        enclosure1.ListAnimals();
        enclosure2.ListAnimals();
        enclosure3.ListAnimals();

        // ========================================
        //     ALL ANIMALS MAKING SOUNDS
        // ========================================
        enclosure1.AllAnimalsSounds();
        enclosure2.AllAnimalsSounds();
        enclosure3.AllAnimalsSounds();

        // ========================================
        //     SPECIFIC ANIMAL BEHAVIORS
        // ========================================
        Console.WriteLine("\n═══ 13. Unique Animal Behaviors ═══");

        Console.WriteLine("\nLion-specific behavior:");
        simba.Hunt();

        Console.WriteLine("\nEagle-specific behavior:");
        freedom.Fly();

        Console.WriteLine("\nSnake-specific behavior:");
        kaa.Slither();
        kaa.ShedSkin();

        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}// ============================================
// INTERFACE: Contract for feedable animals
// ============================================
public interface IFeedable
{
    double CalculateFoodAmount();
    DateTime GetFeedingTime();
}// ============================================
// COMPOSITION: Enclosure contains Animals
// ============================================
public class Enclosure
{
    private int enclosureId;
    private double size;
    private string climate;
    private List<Animal> animals;

    public int EnclosureId => enclosureId;
    public double Size => size;
    public string Climate => climate;
    public List<Animal> Animals => animals;

    public Enclosure(int enclosureId, double size, string climate)
    {
        this.enclosureId = enclosureId;
        this.size = size;
        this.climate = climate;
        this.animals = new List<Animal>();
    }

    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);
        Console.WriteLine($"✓ Added {animal.Name} ({animal.GetType().Name}) to Enclosure {enclosureId}.");
    }

    public void RemoveAnimal(Animal animal)
    {
        if (animals.Remove(animal))
        {
            Console.WriteLine($"✓ Removed {animal.Name} from Enclosure {enclosureId}.");
        }
    }

    public void ListAnimals()
    {
        Console.WriteLine($"\n=== Enclosure {enclosureId} ({climate} climate, {size}m²) ===");
        if (animals.Count == 0)
        {
            Console.WriteLine("No animals in this enclosure.");
        }
        else
        {
            foreach (var animal in animals)
            {
                animal.DisplayInfo();
                Console.WriteLine();
            }
        }
    }

    public void AllAnimalsSounds()
    {
        Console.WriteLine($"\n🔊 Sounds from Enclosure {enclosureId}:");
        foreach (var animal in animals)
        {
            Console.Write($"  {animal.Name}: ");
            animal.MakeSound();
        }
    }
}
public class ZooKeeper
{
    private string name;
    private int employeeId;
    private string assignedSection;

    public string Name => name;
    public int EmployeeId => employeeId;
    public string AssignedSection => assignedSection;

    public ZooKeeper(string name, int employeeId, string assignedSection)
    {
        this.name = name;
        this.employeeId = employeeId;
        this.assignedSection = assignedSection;
    }

    // Method demonstrating polymorphism - works with any Animal
    public void FeedAnimal(Animal animal, string food)
    {
        Console.WriteLine($"\n{name} is feeding the {animal.GetType().Name}...");
        animal.Eat(food);
    }

    // Method that actually updates the animal's health
    public void PerformHealthCheck(Animal animal, string newHealthStatus)
    {
        Console.WriteLine($"\n{name} is performing a health check on {animal.Name}...");
        string oldStatus = animal.HealthStatus;
        animal.HealthStatus = newHealthStatus;
        Console.WriteLine($"Health status updated: {oldStatus} → {newHealthStatus}");
    }

    // Method demonstrating interface usage
    public void PrepareFeedingSchedule(IFeedable feedableAnimal)
    {
        Animal animal = feedableAnimal as Animal;
        Console.WriteLine($"\n{name} is preparing feeding for {animal?.Name}:");
        Console.WriteLine($"  - Food amount needed: {feedableAnimal.CalculateFoodAmount()} kg");
        Console.WriteLine($"  - Feeding time: {feedableAnimal.GetFeedingTime().ToString("h:mm tt")}");
    }
}



public abstract class Animal
{
    // Private fields (ENCAPSULATION)
    private string name;
    private int age;
    private string species;
    private string healthStatus;
    private string feedingSchedule;

    // Public properties with controlled access
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Age
    {
        get { return age; }
        set { age = value; }
    }

    public string Species
    {
        get { return species; }
        set { species = value; }
    }

    public string HealthStatus
    {
        get { return healthStatus; }
        set { healthStatus = value; }
    }

    public string FeedingSchedule
    {
        get { return feedingSchedule; }
        set { feedingSchedule = value; }
    }

    // Constructor
    public Animal(string name, int age, string species, string healthStatus, string feedingSchedule)
    {
        this.name = name;
        this.age = age;
        this.species = species;
        this.healthStatus = healthStatus;
        this.feedingSchedule = feedingSchedule;
    }

    // Abstract method - must be implemented by derived classes
    public abstract void MakeSound();

    // Virtual method - can be overridden
    public virtual void Eat(string foodType)
    {
        Console.WriteLine($"{name} is eating {foodType}.");
    }

    // Regular method
    public void DisplayInfo()
    {
        Console.WriteLine($"Name: {name}, Age: {age}, Species: {species}, Health: {healthStatus}, Feeding: {feedingSchedule}");
    }
}

public class Mammal : Animal
{
    private string furColor;

    public string FurColor
    {
        get { return furColor; }
        set { furColor = value; }
    }

    public Mammal(string name, int age, string species, string healthStatus, string feedingSchedule, string furColor)
        : base(name, age, species, healthStatus, feedingSchedule)
    {
        this.furColor = furColor;
    }

    // POLYMORPHISM: Override abstract method
    public override void MakeSound()
    {
        Console.WriteLine("Generic mammal sound!");
    }

    // POLYMORPHISM: Override virtual method
    public override void Eat(string foodType)
    {
        Console.WriteLine($"{Name} (mammal) is chewing and eating {foodType}.");
    }
}

public class Bird : Animal
{
    private double wingSpan;
    private bool canFly;

    public double WingSpan => wingSpan;
    public bool CanFly => canFly;

    public Bird(string name, int age, string species, string healthStatus, string feedingSchedule, double wingSpan, bool canFly)
        : base(name, age, species, healthStatus, feedingSchedule)
    {
        this.wingSpan = wingSpan;
        this.canFly = canFly;
    }

    public override void MakeSound()
    {
        Console.WriteLine("Bird chirping sound!");
    }

    public override void Eat(string foodType)
    {
        Console.WriteLine($"{Name} (bird) is pecking at {foodType}.");
    }

    public void Fly()
    {
        if (canFly)
        {
            Console.WriteLine($"{Name} is flying with a wingspan of {wingSpan} meters.");
        }
        else
        {
            Console.WriteLine($"{Name} cannot fly.");
        }
    }
}

public class Reptile : Animal
{
    private bool isPoisonous;
    private string scaleType;

    public bool IsPoisonous => isPoisonous;
    public string ScaleType => scaleType;

    public Reptile(string name, int age, string species, string healthStatus, string feedingSchedule, bool isPoisonous, string scaleType)
        : base(name, age, species, healthStatus, feedingSchedule)
    {
        this.isPoisonous = isPoisonous;
        this.scaleType = scaleType;
    }

    public override void MakeSound()
    {
        Console.WriteLine("Reptile hissing sound!");
    }

    public override void Eat(string foodType)
    {
        Console.WriteLine($"{Name} (reptile) is swallowing {foodType} whole.");
    }

    public void ShedSkin()
    {
        Console.WriteLine($"{Name} is shedding its {scaleType} scales.");
    }
}

public class Lion : Mammal, IFeedable
{
    private int prideSize;

    public int PrideSize => prideSize;

    public Lion(string name, int age, string species, string healthStatus, string feedingSchedule, string furColor, int prideSize)
        : base(name, age, species, healthStatus, feedingSchedule, furColor)
    {
        this.prideSize = prideSize;
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} roars loudly: ROARRR!");
    }

    public override void Eat(string foodType)
    {
        Console.WriteLine($"{Name} the lion tears into {foodType} with powerful jaws.");
    }

    public void Hunt()
    {
        Console.WriteLine($"{Name} is hunting with a pride of {prideSize} lions.");
    }

    // IFeedable implementation
    public double CalculateFoodAmount()
    {
        // Lions need about 5-7 kg of meat per day
        return 6.0;
    }

    public DateTime GetFeedingTime()
    {
        // Lions typically feed in the evening
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
    }
}


public class Eagle : Bird, IFeedable
{
    private int huntingSkill;

    public int HuntingSkill => huntingSkill;

    public Eagle(string name, int age, string species, string healthStatus, string feedingSchedule, double wingSpan, bool canFly, int huntingSkill)
        : base(name, age, species, healthStatus, feedingSchedule, wingSpan, canFly)
    {
        this.huntingSkill = huntingSkill;
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} screeches: SCREECH!");
    }

    public override void Eat(string foodType)
    {
        Console.WriteLine($"{Name} the eagle uses its sharp beak to eat {foodType}.");
    }

    public new void Fly()
    {
        Console.WriteLine($"{Name} soars majestically at high altitude with hunting skill level {huntingSkill}.");
    }

    // IFeedable implementation
    public double CalculateFoodAmount()
    {
        // Eagles need about 0.5-1 kg of food per day
        return 0.75;
    }

    public DateTime GetFeedingTime()
    {
        // Eagles typically feed in the morning
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
    }
}


public class Snake : Reptile, IFeedable
{
    private double length;

    public double Length => length;

    public Snake(string name, int age, string species, string healthStatus, string feedingSchedule, bool isPoisonous, string scaleType, double length)
        : base(name, age, species, healthStatus, feedingSchedule, isPoisonous, scaleType)
    {
        this.length = length;
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} hisses menacingly: HISSSS!");
    }

    public override void Eat(string foodType)
    {
        Console.WriteLine($"{Name} the snake unhinging its jaw to swallow {foodType}.");
    }

    public void Slither()
    {
        Console.WriteLine($"{Name} slithers silently, {length} meters long.");
    }

    // IFeedable implementation
    public double CalculateFoodAmount()
    {
        // Snakes need food based on their length (roughly 0.1 kg per meter)
        return length * 0.1;
    }

    public DateTime GetFeedingTime()
    {
        // Snakes typically feed at night
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
    }
}