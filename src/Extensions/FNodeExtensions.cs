namespace CompartmentalizedCreatureGraphics.Extensions;

public static class FNodeExtensions
{
    /// <summary>
    /// MoveInFrontOfOtherNode doesn't work if not in the same container, this function moves the containers to the refrence node's container before moving infront.
    /// </summary>
    /// <param name="thisNode"></param>
    /// <param name="otherNode"></param>
    public static void MoveInFrontOfOtherNodeAndInContainer(this FNode thisNode, FNode otherNode)
    {
        if (otherNode == null)
        {
            Plugin.LogError($"MoveInFrontOfOtherNodeAndInContainer failed, other node is null!");
            return;
        }
        if (otherNode.container == null)
        {
            Plugin.LogError($"MoveInFrontOfOtherNodeAndInContainer failed, other node container is null!");
            return;
        }

        thisNode.RemoveFromContainer();
        otherNode.container.AddChild(thisNode);
        //-- MS7: Force set container for bug fix.
        thisNode._container = otherNode._container;

        thisNode.MoveInFrontOfOtherNode(otherNode);
    }
}
