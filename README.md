# MazeRL

Unity + ML-Agents を使った迷路探索AIのプロジェクトです。

## 📌 概要
エージェントが強化学習によってゴールを目指して迷路を進みます。  
ML-Agents（PPOアルゴリズム）を使用して学習を行いました。

## 🚀 使用技術
- Unity 2022.3
- ML-Agents v0.28.0
- Python 3.8
- PyTorch / NumPy

## 📂 プロジェクト構成
- `Assets`：Unityのシーンやスクリプト
- `TrainingConfigs`：ML-Agents用のYAML設定ファイル
- `Models`：学習済みモデル（未保存の場合は空）

## 🔧 トレーニング方法
```bash
mlagents-learn TrainingConfigs/maze_config.yaml --run-id=maze_run1
