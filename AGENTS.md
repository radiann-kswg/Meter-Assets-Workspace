# AGENTS.md — Meter Assets Workspace

> 本リポジトリで作業する AI エージェント（Claude（Cowork / Claude Code）/ GitHub Copilot / Codex 等）向け指示書の正本（SSOT）。`CLAUDE.md` は本ファイルを指す薄いポインタ。運用ルールの追記は本ファイルにのみ行う。
> ロールプレイ指定は本リポジトリに置かない（Cowork プロジェクト「Unity周り」側の既定＝錦野歌嫁に従う）。
> 2026-09-26 作成（Unity 2022.3.22f1 → 6000.3.23f1 版上げ・Claude バイブコーディング対応と同時）。

## 1. 概要

uGUI 向けのメーター UI アセット集（`Assets/MeterAssets_madebyRadianN/`）。`DemoScene` で各メーターの動作を確認する。

| フォルダ | 内容 |
| --- | --- |
| `LevelGage/` | レベルゲージ（Image + TMP） |
| `ProgressGage/` | 進捗ゲージ（Slider + TMP） |
| `RotaryMeter/` | 回転式メーター（FBX ローター・カウンタ表示） |
| `TargetDistanceMeter/` | ターゲット名・距離・照準アイコン（Canvas 同期） |
| `DemoAnimationDirector.cs` | デモ用の値アニメーション |
| `Assets/TextMeshProReplacer/Editor/` | Text(uGUI) → TMP 置換ツール（`TextReplacer` / `CanvasUIEditor`） |
| `Assets/Editor/GitTools.cs` | `Tools > Git Commit All` / `GitTools.RunGit(...)`（他リポジトリと同一ファイル） |

## 2. 技術スタック

- Unity **6000.3.23f1**（Unity 6.3 LTS）・Built-in Render Pipeline（URP 化しない）・uGUI 2.0（TMP 同梱）
- `com.unity.pipeline` 0.6.0-exp.1（Unity CLI `unity command …` 用）
- ブランチ `main`・リモート `radiann-kswg/Meter-Assets-Workspace`
- `Assets/TextMesh Pro/` は TMP Essentials の取り込み（2026-09-26 に Unity 6 で最初に開いたとき自動更新された。URP/HDRP 用 shadergraph も入るが Built-in RP では未使用）

## 3. コーディング規則

- 回答は日本語。既存の命名（`*Director` / `*Manager`）と `/// <summary>` の日本語コメントを維持する。
- `[SerializeField]` の参照は Inspector で配線する前提。Prefab の配線を変えたらシーンで動作確認する。
- 100 行を超える変更は先に計画を提示する。

## 4. 運用ルール

1. シーン・GameObject・Prefab の操作は Unity CLI（`unity status` → `unity command …`）または Unity MCP 経由。`.unity` / `.prefab` の直接編集は最後の手段。
2. 完了前に Console のエラー（`error CS` を含む）を確認する（`unity command console --level error` / `recompile_status`）。
3. `Library/` `Temp/` `Logs/` `obj/` `UserSettings/` `.vs/` と `.meta` は手で触らない。
4. **サンドボックスから git を書かない**（読むだけなら `GIT_OPTIONAL_LOCKS=0`）。commit は `GitTools.RunGit("add -A")` → `GitTools.RunGit("commit -F Temp/evals/msg.txt")` か User。push は指示があるときだけ。
5. `ProjectSettings.asset` の `organizationId` / `cloudProjectId` は Unity が勝手に書き換える。コミットに含めるかは User 判断。
