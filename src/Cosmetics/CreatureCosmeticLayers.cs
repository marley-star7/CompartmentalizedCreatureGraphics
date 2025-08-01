namespace CompartmentalizedCreatureGraphics.Cosmetics;

public class CreatureCosmeticLayers
{
    protected List<string> _layers = new List<string>();

    public CreatureCosmeticLayers(List<string> _layers)
    {
        
    }

    public IReadOnlyList<string> Layers => _layers.AsReadOnly();

    public void InsertLayerBetween(string newLayer, string[] priorityInFront, string[] priorityBehind)
    {
        if (_layers.Contains(newLayer))
        {
            throw new ArgumentException($"Layer '{newLayer}' already exists");
        }

        // Find first existing layer in the priorityInFront array
        int? frontIndex = null;
        foreach (var layer in priorityInFront)
        {
            int index = _layers.IndexOf(layer);
            if (index != -1)
            {
                frontIndex = index;
                break;
            }
        }

        // Find first existing layer in the priorityBehind array
        int? behindIndex = null;
        foreach (var layer in priorityBehind)
        {
            int index = _layers.IndexOf(layer);
            if (index != -1)
            {
                behindIndex = index;
                break;
            }
        }

        // Determine insertion position
        int insertPos;
        if (frontIndex.HasValue && behindIndex.HasValue)
        {
            if (frontIndex >= behindIndex)
            {
                throw new ArgumentException(
                    $"Priority conflict: Cannot place '{newLayer}' after '{priorityInFront[frontIndex.Value]}' " +
                    $"and before '{priorityBehind[behindIndex.Value]}'");
            }
            insertPos = frontIndex.Value + 1;
        }
        else if (frontIndex.HasValue)
        {
            insertPos = frontIndex.Value + 1;
        }
        else if (behindIndex.HasValue)
        {
            insertPos = behindIndex.Value;
        }
        else
        {
            // If neither priority exists, add to end as fallback
            insertPos = _layers.Count;
        }

        // Ensure we're not inserting after the behindIndex
        if (behindIndex.HasValue && insertPos >= behindIndex.Value)
        {
            insertPos = behindIndex.Value - 1;
        }

        _layers.Insert(insertPos, newLayer);
    }

    // Optional helper method for derived classes to add default layers
    protected void AddDefaultLayer(string layerName)
    {
        if (!_layers.Contains(layerName))
        {
            _layers.Add(layerName);
        }
    }

    protected void AddDefaultLayers(IEnumerable<string> layerNames)
    {
        foreach (var layer in layerNames)
        {
            AddDefaultLayer(layer);
        }
    }
}