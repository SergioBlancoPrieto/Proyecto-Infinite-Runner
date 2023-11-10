using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public bool isGeneratingInitialBlocks;
    public static LevelGenerator sharedInstance;
    public List<LevelBlock> currentLevelBlock = new List<LevelBlock>();
    public List<LevelBlock> allTheLevelBlock = new List<LevelBlock>();
    public Transform levelInitialPoint;

	public void GenerateInitialBlocks()
 	{
		if (currentLevelBlock.Count == 0) 
		{
			isGeneratingInitialBlocks = true;
			for (int i = 0; i<4; i++) {
                AddNewBlock();
            }
			isGeneratingInitialBlocks = false;
		}
		
	}

	public void RemoveAllTheBlocks() 
	{
		while (currentLevelBlock.Count > 0) 
		{
			RemoveOldBlock();
		}
	}

	public void RemoveOldBlock() 
	{
		LevelBlock block = currentLevelBlock[0];
		currentLevelBlock.Remove(block);
		Destroy(block.gameObject);
	}

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }

		GenerateInitialBlocks();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewBlock()
    {
        int randomIndex = Random.Range(0, allTheLevelBlock.Count);

		if (isGeneratingInitialBlocks)
		{
			randomIndex = 0;
		}

        LevelBlock block = (LevelBlock) Instantiate(allTheLevelBlock[randomIndex]);

        block.transform.SetParent(this.transform, false);

        Vector3 blockPosition = Vector3.zero;

        if (currentLevelBlock.Count == 0)
        {
            blockPosition = levelInitialPoint.position;
        }
        else
        {
            blockPosition = currentLevelBlock[currentLevelBlock.Count - 1].exitPoint.position;
        }

        block.transform.position = blockPosition;

        currentLevelBlock.Add(block);
    }
}
