# AGENTS.md — Meter Assets Workspace

> 本リポジトリで作業する AI エージェント（Claude（Cowork / Claude Code）/ GitHub Copilot / Codex 等）向け指示書の正本（SSOT）。`CLAUDE.md` は本ファイルを指す薄いポインタ。運用ルールの追記は本ファイルにのみ行う。
> ロールプレイ指定は本リポジトリに置かない（Cowork プロジェクト「Unity周り」側の既定＝錦野歌嫁に従う）。
> 2026-09-26 作成（Unity 2022.3.22f1 → 6000.3.23f1 版上げ・Claude バイブコーディング対応と同時）。同日に URP 移行・テスト追加・フォントのサブモジュール化・リファクタリングを実施。

## 1. 概要

uGUI 向けのメーター UI アセット集（`Assets/MeterAssets_madebyRadianN/`）。`DemoScene` で各メーターの動作を確認する。

| フォルダ | 内容 |
| --- | --- |
| `LevelGage/` | レベルゲージ（Image + TMP） |
| `ProgressGage/` | 進捗ゲージ（Slider + TMP） |
| `RotaryMeter/` | 回転式メーター（FBX ローター・カウンタ表示） |
| `TargetDistanceMeter/` | ターゲット名・距離・照準アイコン（Canvas 同期） |
| `DemoAnimationDirector.cs` | デモ用の値アニメーション |
| `Tests/Editor/MeterAssetsTests.cs` | EditMode テスト（純関数 `Split` / `DigitPosition` / `FormatDistance` / `IsInView` と引数検証）。asmdef 無し＝Editor アセンブリに載る |
| `Assets/TextMeshProReplacer/Editor/` | Text(uGUI) → TMP 置換ツール（`TextReplacer` / `CanvasUIEditor`） |
| `Assets/Editor/GitTools.cs` | `Tools > Git Commit All` / `GitTools.RunGit(...)`（他リポジトリと同一ファイル） |
| `Assets/Settings/` | URP アセット・レンダラー・Global Settings・DefaultVolumeProfile |
| `Assets/Fonts/` | `PenchantManufacture.otf` / `x14y24pxHeadUpDaisy.ttf` はサブモジュールからのコピー（下記）。TMP SDF は `PenchantManufacture_SDF.asset`（Dynamic・ASCII 印字可能文字を事前登録）と `x14y24pxHeadUpDaisy SDF.asset` |
| `PenchantManufacture_ImageAssets/` | **git サブモジュール**（User 作フォント・CC BY 4.0）。正本は `assets/fonts/PenchantManufacture.otf` |
| `hicchicc.github.io/` | **git サブモジュール**（患者長ひっく氏の x0y0pxFreeFont）。正本は `00ff/x14y24pxHeadUpDaisy.ttf`。ライセンスは配布サイト https://hicchicc.github.io/00ff/ の規約（2026 年に SIL OFL へ移行予定と README にある） |
| `BlenderSources/RotaryMeter.blend` | RotaryMeter の FBX 原本（2021 年の Blender 2.83 ファイルを 2026-09-26 に Blender 5.2 で整理: 画像・マテリアル・メッシュの重複を統合し、テクスチャは `Assets/.../Textures/` を相対参照）。`export_fbx.py` が FBX 書き出しの正本 |

## 2. 技術スタック

- Unity **6000.3.23f1**（Unity 6.3 LTS）・**URP 17.3.0**（2026-09-26 に Built-in から移行。`Assets/Settings/URP_Asset` を Graphics と全 Quality に割当。PPv2・ベイク・反射プローブは元から無し）・uGUI 2.0（TMP 同梱）
- 追加パッケージ（User 導入）: `com.unity.inputsystem` 1.20.0（Active Input Handling = Both。メーター自体は入力を使わない）、`com.unity.recorder` 5.1.7（デモ動画用。Editor クラッシュの前例があるので PNG 連番＋ffmpeg も可）、`com.unity.ai.assistant`（Unity 純正 MCP リレー＝Windows の Claude から `Unity_*` ツールで触るための同梱）
- `com.unity.pipeline` 0.6.0-exp.1（Unity CLI `unity command …` 用）
- ブランチ `main`・リモート `radiann-kswg/Meter-Assets-Workspace`。clone 後は `git submodule update --init`（フォントの正本）
- **フォントの使い分け**: 数字・記号だけの表示（LevelText / Value Text / TargetDistanceText）は PenchantManufacture、日本語が入り得る表示（TargetNameText / ラベル）は x14y24pxHeadUpDaisy。**PenchantManufacture は CJK 未収録**なので日本語を出す TMP に割り当てない
- クレジット: `PenchantManufacture image assets by RadianN_kswg / ラジアン（柏木主税） CC BY 4.0`・`x14y24pxHeadUpDaisy by hicc (x0y0pxFreeFont)`
- `Assets/TextMesh Pro/` は TMP Essentials の取り込み（2026-09-26 に Unity 6 で最初に開いたとき自動更新された。URP/HDRP 用 shadergraph も入るが Built-in RP では未使用）

## 3. コーディング規則

- 回答は日本語。既存の命名（`*Director` / `*Manager`）と `/// <summary>` の日本語コメントを維持する。
- `[SerializeField]` の参照は Inspector で配線する前提。Prefab の配線を変えたらシーンで動作確認する。
- 100 行を超える変更は先に計画を提示する。
- ロジックは MonoBehaviour から切り離した `public static` 純関数に置き、`Tests/Editor/` の EditMode テストで固定する（Test Runner か `unity command run_tests --mode EditMode`）。UI 更新は Set 系メソッド内で行い、`Update` で毎フレーム再描画しない（例外: 対象が動く TargetDistanceMeter / FocusIcon）。

## 4. 運用ルール

1. シーン・GameObject・Prefab の操作は Unity CLI（`unity status` → `unity command …`）または Unity MCP 経由。`.unity` / `.prefab` の直接編集は最後の手段。
2. 完了前に Console のエラー（`error CS` を含む）を確認する（`unity command console --level error` / `recompile_status`）。
3. `Library/` `Temp/` `Logs/` `obj/` `UserSettings/` `.vs/` と `.meta` は手で触らない。
4. **サンドボックスから git を書かない**（読むだけなら `GIT_OPTIONAL_LOCKS=0`）。commit は `GitTools.RunGit("add -A")` → `GitTools.RunGit("commit -F Temp/evals/msg.txt")` か User。push は指示があるときだけ。
5. `ProjectSettings.asset` の `organizationId` / `cloudProjectId` は Unity が勝手に書き換える。コミットに含めるかは User 判断。
6. `unity command capture_game_view --save_path` は `Assets/` 相対に保存して `Assets/Temp/` を作ってしまう（しかもカメラ描画のみで Overlay Canvas の UI は写らない）。UI 込みの画面は Play 中に `eval_file` から `ScreenCapture.CaptureScreenshot("<絶対パス>")` で撮る。`Assets/Temp/` ができていたら消す。
7. `eval_file` は 5 秒でメインスレッド待ちが切れる。`git submodule add` のような長い git はそのまま流すと「timed out」で返るが git 自体は完走する（`Get-Process git` と `git status` で確認）。Play 直後・パッケージ導入中・User が Package Manager を操作中も同様に応答しないので待つ。

## 5. Blender / Blender MCP 運用

- **造形の原本は `BlenderSources/<名前>.blend`、Unity 側は `Assets/MeterAssets_madebyRadianN/<メーター>/Models/*.fbx`**（RSC と同じ運用）。FBX を直したいときは原本を直して `BlenderSources/export_fbx.py` で再エクスポートし、FBX を手で編集しない。
- **RotaryMeter の書き出し規約（`export_fbx.py` に固定・2026-09-26 に既存 FBX と頂点数／三角形数／バウンズ／ノード変換／fileID の一致を確認）**: 1 ファイル 1 オブジェクト（`Box` → `RotalyMater_Box.fbx`、`Roter.000` → `RotalyMater_Roter.fbx`）、`FBX_SCALE_NONE`・`-Z forward / Y up`・bake なし。Unity では node scale 100 × mesh 0.01 になり、`RotalyMeter.prefab` が `RotalyMater_Box` の lscale 1000 で吸収している。**オブジェクト名 `Box` / `Roter.000` はそのまま Unity のメッシュ名＝prefab の参照キー**なので変えない（`.blend` 内のメッシュ・マテリアル・コレクション名は自由）。
- `.blend` 側の 6 本のローターは 1 メッシュ `Roter` を共有（1 本直せば全部に効く）。`Box` の前面には 3 面共有のエッジが 20 本ある（2.83 時代の重ね面。Unity では 5 年間問題なく見えているので放置。造形をやり直すときに直す）。
- Blender MCP（`mcp__remote-devices__Blender__*`）は **Blender GUI を起動して N パネル「BlenderMCP」タブの `Start MCP Server` を押すまで接続できない**（`blender -b` では不可）。セッション開始時にアプリ本体が起動している必要がある。GUI 接続側は `execute_blender_code`、`execute_blender_code_for_cli` は環境変数 `BLENDER_PATH` 未設定だと使えない。
- MCP が無くても `"C:\Program Files (x86)\Steam\steamapps\common\Blenderlender.exe" --background <blend> --python <script.py>`（Blender 5.2 LTS）でヘッドレス実行できる。GUI を塞がずに走らせたいときは `execute_blender_code` から `subprocess.run([bpy.app.binary_path, "--background", ...])`。
- エクスポートの型（RSC AGENTS 罠 21/32 の要点）: `select_all(action='DESELECT')` → 対象のみ選択 → `use_selection=True`、オブジェクト原点はワールド原点（`transform_apply(location)`）、縮退面を残さない。Unity のインポート設定と軸変換は RSC `AGENTS.md` 3 章 19 を参照。
- Unity MCP と Blender MCP を同一セッションで使う場合も、**Unity エディタ／Unity 操作系 MCP は同時に 1 つだけ**の原則は変わらない。
