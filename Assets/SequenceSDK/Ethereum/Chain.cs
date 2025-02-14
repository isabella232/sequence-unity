﻿using System.Numerics;

namespace Sequence
{
    public enum Chain
    {
        // Mainnets
        Ethereum = 1,
        Polygon = 137,
        PolygonZkEvm = 1101,
        BNBSmartChain = 56,
        ArbitrumOne = 42161,
        ArbitrumNova = 42170,
        Optimism = 10,
        Avalanche = 43114,
        Gnosis = 100,
        Base = 8453,
        OasysHomeverse = 19011,
        AstarZKEvm = 3776,
        Xai = 660279,
        
        // Testnets
        TestnetSepolia = 11155111,
        TestnetPolygonAmoy = 80002,
        TestnetArbitrumSepolia = 421614,
        TestnetBNBSmartChain = 97,
        TestnetBaseSepolia = 84532,
        TestnetAvalanche = 43113,
        TestnetOasysHomeverse = 40875,
        TestnetOptimisticSepolia = 11155420,
        TestnetAstarZKyoto = 6038361,
        TestnetXrSepolia = 2730,
        
        TestnetXaiSepolia = -1, // Xai Sepolia's testnet's chain ID is too large to fit inside an int
        
        // Null
        None = 0
    }
}