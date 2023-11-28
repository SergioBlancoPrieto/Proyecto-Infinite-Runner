using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public bool isGeneratingInitialBlocks;
    public static LevelGenerator sharedInstance;
    public List<LevelBlock> currentLevelBlock = new List<LevelBlock>();
    public List<LevelBlock> allTheLevelBlock = new List<LevelBlock>();
    public List<LevelBlock> usableLevelBlocks;
    public LevelBlock lethalBlock;
    public Transform levelInitialPoint;
    private const int NUM_BLOCKS = 4, SCORE_TO_ADD = 100;
    private int score;

	public void GenerateInitialBlocks()
    {
        score = 0;
        usableLevelBlocks = new List<LevelBlock>();
        usableLevelBlocks.AddRange(allTheLevelBlock);
	    usableLevelBlocks.Remove(lethalBlock);

	    isGeneratingInitialBlocks = true;
		for (int i = 0; i<NUM_BLOCKS; i++) {
            AddNewBlock();
            if (isGeneratingInitialBlocks)
            {
	            isGeneratingInitialBlocks = false;
            }
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
	    int score2 = (int) PlayerController.sharedInstance.distanceTravelled / SCORE_TO_ADD;
	    if (score2 > score)
	    {
		    score = score2;
		    usableLevelBlocks.Add(lethalBlock);
	    }
	    
        int randomIndex = Random.Range(0, usableLevelBlocks.Count);

		if (isGeneratingInitialBlocks)
		{
			randomIndex = 0;
		}

        LevelBlock block = (LevelBlock) Instantiate(usableLevelBlocks[randomIndex]);

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
