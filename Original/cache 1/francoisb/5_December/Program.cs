using System.IO.MemoryMappedFiles;

var GetInput = (string inputFileName) =>
{
    List<string> result = new List<string>();
    StreamReader sr = new StreamReader(inputFileName);
    var line = sr.ReadLine();
    while (line != null)
    {
        result.Add(line);
        line = sr.ReadLine();
    }
    sr.Close();
    return result;
};

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (List<string> lines, bool step2) =>
{
    var seedCollection = new List<SeedAllocation>();
    var seedRangeCollection = new List<SeedRangeAllocation>();
    var SeedToSoil = new List<Map>();
    var SoilToFertilizer = new List<Map>();
    var FertilizerToWater = new List<Map>();
    var WaterToLight = new List<Map>();
    var LightToTemperature = new List<Map>();
    var TemperatureToHumidity = new List<Map>();
    var HumidityToLocation = new List<Map>();
    var sectionName = "";
    foreach (var line in lines)
    {
        if(line.Contains(':')) // new Section
        {
            var splitLine = line.Split(':');
            if (string.Equals(splitLine[0], "seeds", StringComparison.InvariantCultureIgnoreCase))
            {
                var numLine = splitLine[1].Trim().Split(' ');
                if(step2)
                {
                    for (var x = 0; x < numLine.Count(); x += 2)
                    {
                        var seedStart = long.Parse(numLine[x]);
                        var seedRange = long.Parse(numLine[x + 1])-1;

                        var seed = new SeedRangeAllocation(seedStart, seedStart + seedRange, seedRange);
                        seedRangeCollection.Add(seed);
                    }
                }
                else
                {
                    foreach (var item in numLine)
                    {
                        var seedNum = long.Parse(item);
                        var seed = new SeedAllocation(seedNum);
                        seedCollection.Add(seed);
                    }
                }
            }

            sectionName = splitLine[0];
        } 
        else if (!string.IsNullOrEmpty(line))
        {
            
            var section = line.Split(' ');
            var dest = long.Parse(section[0]);
            var source = long.Parse(section[1]);
            var range = long.Parse(section[2]);
            var destEnd = dest + range-1;
            var sourceEnd = source + range-1;
            switch(sectionName)
            {
                case "seed-to-soil map":
                    SeedToSoil.Add(new Map(dest, destEnd, source, sourceEnd, range));
                    break;
                case "soil-to-fertilizer map":
                    SoilToFertilizer.Add(new Map(dest, destEnd, source, sourceEnd, range));
                    break;
                case "fertilizer-to-water map":
                    FertilizerToWater.Add(new Map(dest, destEnd, source, sourceEnd, range));
                    break;
                case "water-to-light map":
                    WaterToLight.Add(new Map(dest, destEnd, source, sourceEnd, range));
                    break;
                case "light-to-temperature map":
                    LightToTemperature.Add(new Map(dest, destEnd, source, sourceEnd, range));
                    break;
                case "temperature-to-humidity map":
                    TemperatureToHumidity.Add(new Map(dest, destEnd, source, sourceEnd, range));
                    break;
                case "humidity-to-location map":
                    HumidityToLocation.Add(new Map(dest, destEnd, source, sourceEnd, range));
                    break;
                default:
                    break;
            }
        }
    }

    return new Collection(seedCollection, seedRangeCollection, SeedToSoil, SoilToFertilizer, FertilizerToWater, WaterToLight, LightToTemperature, TemperatureToHumidity, HumidityToLocation);
};

var ProcessInputStep1 = (List<string> lines, bool step2) =>
{
    var collection = GenerateCollections(lines, step2);

    collection.SeedCollection.ForEach(seed =>
    {
        var soilMap = collection.SeedToSoil.Where(q => q.SourceStart <= seed.Seed && q.SourceEnd >= seed.Seed).FirstOrDefault();
        if (soilMap == null) seed.Soil = seed.Seed;
        else
        {
            var position = seed.Seed - soilMap.SourceStart;
            seed.Soil = soilMap.DestinationStart + position;
        }

        var fertMap = collection.SoilToFertilizer.Where(q => q.SourceStart <= seed.Soil && q.SourceEnd >= seed.Soil).FirstOrDefault();
        if (fertMap == null) seed.Fertilizer = seed.Soil;
        else
        {
            var position = seed.Soil - fertMap.SourceStart;
            seed.Fertilizer = fertMap.DestinationStart + position;
        }

        var waterMap = collection.FertilizerToWater.Where(q => q.SourceStart <= seed.Fertilizer && q.SourceEnd >= seed.Fertilizer).FirstOrDefault();
        if (waterMap == null) seed.Water = seed.Fertilizer;
        else
        {
            var position = seed.Fertilizer - waterMap.SourceStart;
            seed.Water = waterMap.DestinationStart + position;
        }

        var lightMap = collection.WaterToLight.Where(q => q.SourceStart <= seed.Water && q.SourceEnd >= seed.Water).FirstOrDefault();
        if (lightMap == null) seed.Light = seed.Water;
        else
        {
            var position = seed.Water - lightMap.SourceStart;
            seed.Light = lightMap.DestinationStart + position;
        }

        var tempMap = collection.LightToTemperature.Where(q => q.SourceStart <= seed.Light && q.SourceEnd >= seed.Light).FirstOrDefault();
        if (tempMap == null) seed.Temperature = seed.Light;
        else
        {
            var position = seed.Light - tempMap.SourceStart;
            seed.Temperature = tempMap.DestinationStart + position;
        }

        var humMap = collection.TemperatureToHumidity.Where(q => q.SourceStart <= seed.Temperature && q.SourceEnd >= seed.Temperature).FirstOrDefault();
        if (humMap == null) seed.Humidity = seed.Temperature;
        else
        {
            var position = seed.Temperature - humMap.SourceStart;
            seed.Humidity = humMap.DestinationStart + position;
        }

        var locMap = collection.HumidityToLocation.Where(q => q.SourceStart <= seed.Humidity && q.SourceEnd >= seed.Humidity).FirstOrDefault();
        if (locMap == null) seed.Location = seed.Humidity;
        else
        {
            var position = seed.Humidity - locMap.SourceStart;
            seed.Location = locMap.DestinationStart + position;
        }
    });

    return collection;
};

var MapSrcToDest = (List<Map> Maps, List<long> location) =>
{
    var last = location.Last();
    var matching = Maps.Where(q => q.SourceStart <= last && q.SourceEnd >= last).FirstOrDefault();
    if (matching != null)
    {
        long diff = last - matching.SourceStart;
        location.Add(matching.DestinationStart + diff);
    }
    else
    {
        location.Add(last);
    }
};

var ProcessInputStep2 = (List<string> lines) =>
{
    var collection = GenerateCollections(lines, true);

    var minLocation = long.MaxValue;

    collection.SeedRangeCollection.ForEach(q =>
    {
        for(long y = q.Seed; y < q.Seed + q.SeedRange; y++)
        {
            var locations = new List<long>();
            locations.Add(y);

            MapSrcToDest(collection.SeedToSoil, locations);
            MapSrcToDest(collection.SoilToFertilizer, locations);
            MapSrcToDest(collection.FertilizerToWater, locations);
            MapSrcToDest(collection.WaterToLight, locations);
            MapSrcToDest(collection.LightToTemperature, locations);
            MapSrcToDest(collection.TemperatureToHumidity, locations);
            MapSrcToDest(collection.HumidityToLocation, locations);

            minLocation = Math.Min(locations.Last(), minLocation);
        }
    });


    return minLocation;
};

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input, false);
var result = "Step 1: " + CalValues.SeedCollection.Min(q => q.Location);
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);

public class SeedAllocation(long seedStart)
{
    public long Seed { get; set; } = seedStart;
    public long Soil { get; set; }
    public long Fertilizer { get; set; }
    public long Water { get; set; }
    public long Light { get; set; }
    public long Temperature { get; set; }
    public long Humidity { get; set; }
    public long Location { get; set; }
}

public class SeedRangeAllocation(long seedStart, long seedEnd, long seedRange)
{
    public long Seed { get; set; } = seedStart;
    public long SeedEnd { get; set; } = seedEnd;
    public long SeedRange { get; set; } = seedRange;
    public long Location { get; set; }
}


public class Collection(List<SeedAllocation> seeds, List<SeedRangeAllocation> seedRange, List<Map> sts, List<Map> stf, List<Map> ftw, List<Map> wtl, List<Map> ltt, List<Map> tth, List<Map> htl)
{
    public List<SeedAllocation> SeedCollection { get; set; } = seeds;
    public List<SeedRangeAllocation> SeedRangeCollection { get; set; } = seedRange;
    public List<Map> SeedToSoil { get; set; } = sts;
    public List<Map> SoilToFertilizer { get; set; } = stf;
    public List<Map> FertilizerToWater { get; set; } = ftw;
    public List<Map> WaterToLight { get; set; } = wtl;
    public List<Map> LightToTemperature { get; set; } = ltt;
    public List<Map> TemperatureToHumidity { get; set; } = tth;
    public List<Map> HumidityToLocation { get; set; } = htl;
}

public class Map(long destStart, long destEnd, long sourceStart, long sourceEnd, long range)
{
    public long DestinationStart { get; set; } = destStart;
    public long DestinationEnd { get; set; } = destEnd;
    public long SourceStart { get; set; } = sourceStart;
    public long SourceEnd { get; set; } = sourceEnd;
    public long Range { get; set; } = range;
}
