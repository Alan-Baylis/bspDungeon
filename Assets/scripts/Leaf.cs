using UnityEngine;

public class Leaf : MonoBehaviour {

	private int minLeafSize = 50;

    public int x, y, width, height;

    public Leaf leftChild, rightChild;
    public bool hasRoom;
    public Vector2 roomSize;
    public Vector2 roomPos;

    private GameObject quad;

    public Leaf (int _x, int _y, int _width, int _height)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;

        quad = GameObject.CreatePrimitive (PrimitiveType.Quad);
        quad.transform.position = new Vector3 (x + (width * 0.5f), 0, y + (height * 0.5f));
        quad.transform.localScale = new Vector3 (width, 1f, height);

        Color randColor = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1f);
        quad.GetComponent<Renderer> ().material.color = randColor;
    }

    public bool Split ()
    {
        // If the leaf already has children, skip it
        if (leftChild != null || rightChild != null) {
            return false;
        }

        // 50% chance of splitting horizontally
        bool splitH = Random.Range (0f, 1f) < 0.5f;
        
        // If the room is wider than it is tall, split vertically
        if (width > height && height / width >= 0.5f)
        {
            splitH = false;
        }
        else if (height > width && width / height >= 0.5f)
        {
            splitH = true;
        }

        // Determine the max size of the new leaf
        int max = (splitH ? height : width) - minLeafSize;
        
        // If it's too small, don't split
        if (max <= minLeafSize)
        {
            return false;
        }

        // Generate new dimension for the split
        int split = (int) Random.Range (minLeafSize, max);

        // Split
        if (splitH)
        {
            leftChild = new Leaf (x, y, width, split);
            rightChild = new Leaf (x, y + split, width, height - split);
        }
        else
        {
            leftChild = new Leaf (x, y, split, height);
            rightChild = new Leaf (x + split, y, width - split, height);
        }

        if (quad != null)
        {
            GameObject.Destroy (quad);
            quad = null;
        }

        // return true if split successfully
        return true;
    }

    public void CreateRooms ()
    {
        if (leftChild != null || rightChild != null)
        {
            if (leftChild != null)
            {
                leftChild.CreateRooms ();
            }
            if (rightChild != null)
            {
                rightChild.CreateRooms ();
            }
            hasRoom = false;
        }
        else
        {
            roomSize = new Vector2 (Random.Range (3, width - 2), Random.Range (3, height - 2));
            roomPos = new Vector2 (Random.Range (2, width - roomSize.x - 2), Random.Range (2, height - roomSize.y - 2));
            hasRoom = true;
        }
        Debug.Log (leftChild);
        Debug.Log (rightChild);
        Debug.Log (hasRoom);
    }

}
