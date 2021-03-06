using System;
using UnityEngine;
using System.Collections;

public class FileTerrainGenerator
{
	private string _fullMapPath;
	private Map _map;
	private const string _baseMapFolder = "Assets/Maps";
	
	public FileTerrainGenerator (Map map, string mapName)
	{
		_map = map;
		_fullMapPath = System.IO.Path.Combine (_baseMapFolder, mapName);
	}
	
	public void LoadLevel()
	{
		int verticalOffset = 85;
		
		Debug.Log ("About to load level folder: " + _fullMapPath + ".");
		
		Substrate.AnvilWorld mcWorld = Substrate.AnvilWorld.Create (_fullMapPath);
			
		Substrate.AnvilRegionManager mcAnvilRegionManager = mcWorld.GetRegionManager();
				
		BlockSet blockSet = _map.GetBlockSet();
				
		_map.GetSunLightmap().SetSunHeight(3, 3, 3);
		
		foreach( Substrate.AnvilRegion mcAnvilRegion in mcAnvilRegionManager )
		{
			// Loop through x-axis of chunks in this region
			for (int iMCChunkX  = 0; iMCChunkX < mcAnvilRegion.XDim; iMCChunkX++)
			{
				// Loop through z-axis of chunks in this region.
				for (int iMCChunkZ = 0; iMCChunkZ < mcAnvilRegion.ZDim; iMCChunkZ++)
				{
					// Retrieve the chunk at the current position in our 2D loop...
					Substrate.ChunkRef mcChunkRef = mcAnvilRegion.GetChunkRef (iMCChunkX, iMCChunkZ);
					
					if (mcChunkRef != null)
					{
						if (mcChunkRef.IsTerrainPopulated)
						{
							// Ok...now to stick the blocks in...
							
							for (int iMCChunkInternalX = 0; iMCChunkInternalX < mcChunkRef.Blocks.XDim; iMCChunkInternalX++)
							{
								for (int iMCChunkInternalY = 0; iMCChunkInternalY < mcChunkRef.Blocks.YDim; iMCChunkInternalY++)
								{
									for (int iMCChunkInternalZ = 0; iMCChunkInternalZ < mcChunkRef.Blocks.ZDim; iMCChunkInternalZ++)
									{
										int iBlockID = mcChunkRef.Blocks.GetID (iMCChunkInternalX, iMCChunkInternalY, iMCChunkInternalZ);
										
										Vector3i blockPos = new Vector3i(iMCChunkInternalX, iMCChunkInternalY + verticalOffset, iMCChunkInternalZ);
										
										if (iBlockID != 0)
										{
											switch (iBlockID)
											{
											case 3: // Dirt to first grass
												iBlockID = 1;
												break;
											case 12: // Grass to grass
												iBlockID = 1;
												break;
											case 13: // Gravel to stone
												iBlockID = 4;
												break;
											case 1: // Stone to second stone
												iBlockID = 5;
												break;
											case 16: // Coal ore to fungus
												iBlockID = 17;
												break;
											case 15: // Iron ore to pumpkin
												iBlockID = 20;
												break;
											case 9: // Water to water
												iBlockID = 8;
												break;
											default:
												Debug.Log ("Unmapped BlockID: " + iBlockID);
												break;
											}
											
											Block newBlock = blockSet.GetBlock(iBlockID);
																					
											_map.SetBlock (new BlockData(newBlock), blockPos);
											
											Vector3i chunkPos = Chunk.ToChunkPosition(blockPos);
				
											_map.SetDirty (chunkPos);	
										}
									} // End for (int iMCChunkInternalZ = 0; iMCChunkInternalZ < mcChunkRef.Blocks.ZDim; iMCChunkInternalZ++)
								} // End for (int iMCChunkInternalY = 0; iMCChunkInternalY < mcChunkRef.Blocks.YDim; iMCChunkInternalY++)
							} // End for (int iMCChunkInternalX = 0; iMCChunkInternalX < mcChunkRef.Blocks.XDim; iMCChunkInternalX++)
						} // End if (mcChunkRef.IsTerrainPopulated)
					} // End if (mcChunkRef != null)
				} // End for (int iMCChunkZ = 0; iMCChunkZ < mcAnvilRegion.ZDim; iMCChunkZ++)
			} // End for (int iMCChunkX  = 0; iMCChunkX < mcAnvilRegion.XDim; iMCChunkX++)
		} // End foreach( Substrate.AnvilRegion mcAnvilRegion in mcAnvilRegionManager )
		
		Debug.Log ("Loaded level: " + _fullMapPath + ".");
		
		_map.AddColliders ();
		
//		List3D<Chunk> chunkList = _map.GetChunks ();
//		
//		for (int minX = chunkList.GetMinX(); minX < chunkList.GetMaxX(); minX++)
//		{
//			for (int minY = chunkList.GetMinY(); minY < chunkList.GetMaxY(); minY++)
//			{
//				for (int minZ = chunkList.GetMinZ(); minZ < chunkList.GetMaxZ(); minZ++)
//				{
//					Vector3i chunkPos = new Vector3i(minX, minY, minZ);
//					
//					Chunk mapChunk = _map.GetChunk (chunkPos);
//					
//					Debug.Log ("LOL [" + minX + ", " + minY + ", " + minZ + "].");
//				}	
//			}	
//		}
//		
//		foreach (Chunk loadedChunk in _map.Get
//		{
//			Vector3i loadedChunkPos = loadedChunk.GetPosition();
//			Debug.Log ("MOG A CHUNK!!! [" + loadedChunkPos.x + ", " + loadedChunkPos.y + ", " + loadedChunkPos.z + "].");
//		}

	} // End public void LoadLevel()
	
//	public void LoadLevelBackup()
//	{
//		int verticalOffset = 85;
//		
//		Debug.Log ("About to load level folder: " + _fullMapPath + ".");
//		
//		Substrate.AnvilWorld mcWorld = Substrate.AnvilWorld.Create (_fullMapPath);
//			
//		Substrate.AnvilRegionManager mcAnvilRegionManager = mcWorld.GetRegionManager();
//				
//		BlockSet blockSet = _map.GetBlockSet();
//		
//		Block[] blockSetBlock = blockSet.GetBlocks();
//		
//		//Debug.Log ("BlockSet size: " + blockSet.GetBlockCount ());
//		
//		for (int iBlockSet = 0; iBlockSet < blockSetBlock.Length; iBlockSet++)
//		{
//			//Debug.Log("BlockSet[" + iBlockSet + "] contains " + blockSetBlock[iBlockSet].GetName ());	
//		}
//				
//		_map.GetSunLightmap().SetSunHeight(3, 3, 3);
//		
//		foreach( Substrate.AnvilRegion mcAnvilRegion in mcAnvilRegionManager )
//		{
//			int regionChunkCount = mcAnvilRegion.ChunkCount ();
//			
//			//Debug.Log ("Number of chunks in region: " + regionChunkCount);
//			
//			// Loop through x-axis of chunks in this region
//			for (int iMCChunkX  = 0; iMCChunkX < mcAnvilRegion.XDim; iMCChunkX++)
//			{
//				//Debug.Log ("Chunk looper, x: " + iChunkX);	
//				
//				// Loop through z-axis of chunks in this region.
//				for (int iMCChunkZ = 0; iMCChunkZ < mcAnvilRegion.ZDim; iMCChunkZ++)
//				{
//					//Debug.Log ("Chunk looper, z: " + iChunkZ);	
//					
//					// Retrieve the chunk at the current position in our 2D loop...
//					Substrate.ChunkRef mcChunkRef = mcAnvilRegion.GetChunkRef (iMCChunkX, iMCChunkZ);
//					
//					if (mcChunkRef != null)
//					{
//						if (mcChunkRef.IsTerrainPopulated)
//						{
//							// Ok...now to stick the blocks in...
//							
//							//Debug.Log ("MC Chunk at [" + iMCChunkX + ", 0, " + iMCChunkZ + "] has dimensions [" + mcChunkRef.Blocks.XDim + ", " + mcChunkRef.Blocks.YDim + ", " + mcChunkRef.Blocks.ZDim + "]");
//							
//							for (int iMCChunkInternalX = 0; iMCChunkInternalX < mcChunkRef.Blocks.XDim; iMCChunkInternalX++)
//							{
//								for (int iMCChunkInternalY = 0; iMCChunkInternalY < mcChunkRef.Blocks.YDim; iMCChunkInternalY++)
//								{
//									for (int iMCChunkInternalZ = 0; iMCChunkInternalZ < mcChunkRef.Blocks.ZDim; iMCChunkInternalZ++)
//									{
////										Chunk chunkRef = _map.GetChunk(chunkRefPos);
//										
//										int iBlockID = mcChunkRef.Blocks.GetID (iMCChunkInternalX, iMCChunkInternalY, iMCChunkInternalZ);
//										
//										Vector3i blockPos = new Vector3i(iMCChunkInternalX, iMCChunkInternalY + verticalOffset, iMCChunkInternalZ);
//										
//										if (iBlockID != 0)
//										{
//											switch (iBlockID)
//											{
//											case 3: // Dirt to first grass
//												iBlockID = 1;
//												break;
//											case 12: // Grass to grass
//												iBlockID = 1;
//												break;
//											case 13: // Gravel to stone
//												iBlockID = 4;
//												break;
//											case 1: // Stone to second stone
//												iBlockID = 5;
//												break;
//											case 16: // Coal ore to fungus
//												iBlockID = 17;
//												break;
//											case 15: // Iron ore to pumpkin
//												iBlockID = 20;
//												break;
//											case 9: // Water to water
//												iBlockID = 8;
//												break;
//											default:
//												Debug.Log ("Unmapped BlockID: " + iBlockID);
//												break;
//											}
//											
//											Block newBlock = blockSet.GetBlock(iBlockID);
//																					
//											_map.SetBlock (new BlockData(newBlock), blockPos);
//												
//											Vector3i chunkPos = Chunk.ToChunkPosition(blockPos);
//				
//											_map.SetDirty (chunkPos);	
//										}
//									} // End for (int iMCChunkInternalZ = 0; iMCChunkInternalZ < mcChunkRef.Blocks.ZDim; iMCChunkInternalZ++)
//								} // End for (int iMCChunkInternalY = 0; iMCChunkInternalY < mcChunkRef.Blocks.YDim; iMCChunkInternalY++)
//							} // End for (int iMCChunkInternalX = 0; iMCChunkInternalX < mcChunkRef.Blocks.XDim; iMCChunkInternalX++)
//						} // End if (mcChunkRef.IsTerrainPopulated)
//					} // End if (mcChunkRef != null)
//				} // End for (int iMCChunkZ = 0; iMCChunkZ < mcAnvilRegion.ZDim; iMCChunkZ++)
//			} // End for (int iMCChunkX  = 0; iMCChunkX < mcAnvilRegion.XDim; iMCChunkX++)
//		} // End foreach( Substrate.AnvilRegion mcAnvilRegion in mcAnvilRegionManager )
//		
//		Debug.Log ("Loaded level: " + _fullMapPath + ".");
//
//	} // End public void LoadLevel()
	
}