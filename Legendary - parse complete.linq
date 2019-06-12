<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	//var path = @"D:\__SOURCES\legendary-complete-card-txt\2_heroes_and_allies.txt";
	var path = @"G:\__Code\legendary-complete-card-txt\2_heroes_and_allies.txt";
	
	var lines = File.ReadAllLines(path);

	ProcessSet(FindSet("World War Hulk", lines)).Dump("WWH");
	ProcessSet(FindSet("Ant-Man", lines)).Dump("Ant-Man");
	ProcessSet(FindSet("Venom", lines)).Dump("Venom");
}

public IEnumerable<Line> ProcessSet(IEnumerable<string> lines)
{
	var l = new Line { CardType = "Hero" } ;
	var Lines = new List<Line> { l };
	var set = string.Empty;
	var hero = string.Empty;
	var inCard = false;
	var inEffect = false;

	foreach (var line in lines)
	{
		$"-{line}-".Dump();
		if (string.IsNullOrWhiteSpace(line.Trim()))
		{
			if (!inCard) continue;
			if (inEffect)
			{
				l = new Line
				{
					Set = set,
					GroupName = hero,
					CardType = "Hero",
				};

				Lines.Add(l);
				inEffect = false;
				inCard = false;
				continue;
			}
		}
		else
		{
			if (inEffect)
			{
				l.Text.Add(line);
				continue;
			}
		}

		var prefix = line.Substring(0, 3);
		var data = line.Substring(3);


		switch (prefix)
		{
			case "ST:":
				l.Set = set = data;
				break;
			case "HN:":
				l.GroupName = hero = data;
				break;
			case "QT:":
				l.Qty = data;
				inCard = true;
				break;
			case "CN:":
				l.CardName = data;
				break;
			case "TN:":
				l.TeamIcon = data;
				break;
			case "CL:":
				l.Class = data;
				break;
			case "AT:":
				l.Attack = data;
				break;
			case "RE:":
				l.Recruit = data;
				break;
			case "CO:":
				l.Cost = data;
				break;
			case "EF:":
				inEffect = true;
				break;
		}
	}

	return Lines;
}

public IEnumerable<string> FindSet(string setName, string[] lines) 
{
	var inSet = false;
	var setLines = new List<string>();
	foreach(var line in lines)
	{
		if (line.StartsWith($"ST:{setName}")) inSet = true;
		else if (line.StartsWith("ST:") && inSet) break;
		
		if(inSet) setLines.Add(line);
	}
	
	if(string.IsNullOrWhiteSpace(setLines.Last())) setLines.RemoveAt(setLines.Count - 1);	
	
	return setLines;
}

// Define other methods and classes here
public class Line
{
	public string Qty;
	public string CardName;
	public string GroupName;
	public string CardType;
	public string Class;
	public string Cost;
	public string Recruit;
	public string Attack;
	public string TeamIcon;
	public string VP;
	public List<string> Text = new List<string>();
	public string Set;

}