var input = ParseInput();

Console.WriteLine($"Part 1: {input.Updates.Where(x => IsInOrder(x, input.Rules)).Select(x => x[(x.Count - 1) / 2]).Sum()}");
Console.WriteLine($"Part 2: {input.Updates.Where(x => !IsInOrder(x, input.Rules)).Select(x => FixOrder(x, input.Rules)).Select(x => x[(x.Count - 1) / 2]).Sum()}");

static bool IsInOrder(List<int> update, Dictionary<int, PageRule> rules)
{
    for (var i = 0; i < update.Count; i++)
    {
        var pageRules = rules[update[i]];

        var notInOrder = pageRules.After.Intersect(update.Skip(i + 1)).Any() || pageRules.Before.Intersect(update.Take(i)).Any();

        if (notInOrder)
        {
            return false;
        }
    }

    return true;
}

static List<int> FixOrder(List<int> update, Dictionary<int, PageRule> rules)
{
    var newOrder = new List<int>();

    foreach (var toInsert in update)
    {
        var index = GetIndex(newOrder.Count / 2, newOrder.Count / 2, toInsert, newOrder, rules);
        if (index < newOrder.Count)
        {
            newOrder.Insert(index, toInsert);
        }
        else
        {
            newOrder.Add(toInsert);
        }
    }

    return newOrder;
}

static int GetIndex(int index, int prevIndex, int page, List<int> pages, Dictionary<int, PageRule> rules)
{
    if (pages.Count == 0 || index < 0)
    {
        return 0;
    }
    else if (index >= pages.Count)
    {
        return pages.Count;
    }
    else
    {
        var indexRules = rules[pages[index]];

        if (indexRules.After.Contains(page))
        {
            return prevIndex < index ? index : GetIndex(index - 1, index, page, pages, rules);
        }
        else if (indexRules.Before.Contains(page))
        {
            return prevIndex > index ? prevIndex : GetIndex(index + 1, index, page, pages, rules);
        }

        return index;
    }
}


static Input ParseInput()
{
    var lines = File.ReadAllLines("input.txt");
    var input = new Input([], []);
    var parsingRules = true;
    foreach (var line in lines)
    {
        if (line == "")
        {
            parsingRules = false;
            continue;
        }

        if (parsingRules)
        {
            var pages = line.Split('|').Select(int.Parse);
            var left = pages.First();
            var right = pages.Last();

            var rule = new PageRule([], []);

            if (input.Rules.TryGetValue(left, out rule))
            {
                rule.Before.Add(right);
            }
            else
            {
                input.Rules.Add(left, new PageRule([], [right]));
            }

            if (input.Rules.TryGetValue(right, out rule))
            {
                rule.After.Add(left);
            }
            else
            {
                input.Rules.Add(right, new PageRule([left], []));
            }
        }
        else
        {
            input.Updates.Add(line.Split(',').Select(int.Parse).ToList());
        }
    }

    return input;
}
record Input(Dictionary<int, PageRule> Rules, List<List<int>> Updates);
record PageRule(List<int> After, List<int> Before);
