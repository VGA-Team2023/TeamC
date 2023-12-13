public enum BGMState
{
    /// <summary>タイトル</summary>
    Title,
    /// <summary>チュートリアル</summary>
    Tutorial,
    /// <summary>バトル</summary>
    Battle,
    /// <summary>リザルト</summary>
    Result,
}
public enum SEState
{
    /// <summary>Player通常攻撃発射音(氷)</summary>
    PlayerShootIce,
    /// <summary>Player通常攻撃発射音(草)</summary>
    PlayerShootGrass,
    /// <summary>Player必殺技チャージ音(氷)</summary>
    PlayerChargeIce,
    /// <summary>Player必殺技チャージ音(草)</summary>
    PlayerChargeGrass,
    /// <summary>Playerトレイル音(氷パターン1)</summary>
    PlayerTrailIcePatternA,
    /// <summary>Playerトレイル音(氷パターン2)</summary>
    PlayerTrailIcePatternB,
    /// <summary>Playerトレイル音(草パターン1)</summary>
    PlayerTrailGrassPatternA,
    /// <summary>Playerトレイル音(草パターン2)</summary>
    PlayerTrailGrassPatternB,
    /// <summary>Player魔法陣展開音</summary>
    PlayerMagiccircl,
    /// <summary>Player回避音(氷)</summary>
    PlayerDodgeIce,
    /// <summary>Player回避音(草)</summary>
    PlayerDodgeGrass,
    /// <summary>Player遠距離敵からのダメージ</summary>
    PlayerLongAttackEnemyDamage,


    /// <summary>Player通常攻撃が当たった時のHit音(氷パターン1)</summary>
    EnemyHitIcePatternA,
    /// <summary>Player通常攻撃が当たった時のHit音(氷パターン2)</summary>
    EnemyHitIcePatternB,
    /// <summary>Player通常攻撃が当たった時のHit音(草パターン1)</summary>
    EnemyHitGrassPatternA,
    /// <summary>Player通常攻撃が当たった時のHit音(草パターン2)</summary>
    EnemyHitGrassPatternB,
    /// <summary>Player必殺攻撃が当たった時のHit音(氷)</summary>
    EnemyFinichHitIce,
    /// <summary>Player必殺攻撃が当たった時のHit音(草)</summary>
    EnemyFinishHitGrass,
    /// <summary>近敵の攻撃音</summary>
    EnemyCloseAttack,
    /// <summary>遠敵の攻撃発射音</summary>
    EnemyLongAttackShoot,
    /// <summary>遠敵の攻撃のトレイル音</summary>
    EnemyLongAttackTrail,
    /// <summary>Playerの通常攻撃時のダメージ音</summary>
    EnemyNormalDamage,
    /// <summary>Playerのトドメ攻撃時のダメージ音</summary>
    EnemyFinishDamage,
    /// <summary>スタン音</summary>
    EnemyStan,
    /// <summary>登場音</summary>
    EnemyIn,
    /// <summary>退場音</summary>
    EnemyOut,


    /// <summary>システムの決定音</summary>
    SystemApply,
    /// <summary>システムのキャンセル音</summary>
    SystemCancel,
}

public enum VoiceState
{
    /// <summary>通常攻撃(はっ！)</summary>
    PlayerAttackPattern1,
    /// <summary>通常攻撃(ふっ！)</summary>
    PlayerAttackPattern2,
    /// <summary>通常攻撃(やっ！)</summary>
    PlayerAttackPattern3,
    /// <summary>通常攻撃(これで！)</summary>
    PlayerAttackPattern4,
    /// <summary>魔法陣展開(まだまだ！)</summary>
    PlayerCastingNormalPattern1,
    /// <summary>魔法陣展開(次・・・！)</summary>
    PlayerCastingNormalPattern2,
    /// <summary>氷魔法陣展開(グレイシオ！)</summary>
    PlayerCastingIce,
    /// <summary>草魔法陣展開(ロゼエラ！)</summary>
    PlayerCastingGrass,
    /// <summary>氷魔法チャージ中(冷気よ・・・！)</summary>
    PlayerChargeIcePattern1,
    /// <summary>氷魔法チャージ中(氷よ・・・！)</summary>
    PlayerChargeIcePattern2,
    /// <summary>氷魔法チャージ中(凍れ・・・！)</summary>
    PlayerChargeIcePattern3,
    /// <summary>草魔法チャージ中(薔薇よ・・・！)</summary>
    PlayerChargeGrassPattern1,
    /// <summary>草魔法チャージ中(緑よ・・・！)</summary>
    PlayerChargeGrassPattern2,
    /// <summary>草魔法チャージ中(命よ・・・！)</summary>
    PlayerChargeGrassPattern3,
    /// <summary>必殺攻撃(トドメ！)</summary>
    PlayerFinishNormalPattern1,
    /// <summary>必殺攻撃(これで終わり！)</summary>
    PlayerFinishNormalPattern2,
    /// <summary>必殺攻撃(コアを！)</summary>
    PlayerFinishNormalPattern3,
    /// <summary>氷魔法必殺攻撃(熱を奪う！)</summary>
    PlayerFinishIce,
    /// <summary>草魔法必殺攻撃(芽吹け・・・！)</summary>
    PlayerFinishGrass,
    /// <summary>回避(せっ！)</summary>
    PlayerDodgePattern1,
    /// <summary>回避(っと！)</summary>
    PlayerDodgePattern2,
    /// <summary>回避(っしょ！)</summary>
    PlayerDodgePattern3,
    /// <summary>ダメージ(うっ！)</summary>
    PlayerDamagePattern1,
    /// <summary>ダメージ(痛った！)</summary>
    PlayerDamagePattern2,
    /// <summary>ダメージ(ぐっ！)</summary>
    PlayerDamagePattern3,
    /// <summary>HP0(こんな・・・ところで・・・)</summary>
    PlayerDownPattern1,
    /// <summary>HP0(まだ・・・戦える・・・)</summary>
    PlayerDownPattern2,
    /// <summary>HP0(悔しい！)</summary>
    PlayerDownPattern3,

    /// <summary>索敵(オオオオォォ・・・・)</summary>
    EnemySaerch,
    /// <summary>Player発見(キッ！)</summary>
    EnemyDiscovPattern1,
    /// <summary>Player発見(ヒヒヒ！)</summary>
    EnemyDiscovPattern2,
    /// <summary>攻撃(キヒャ！)</summary>
    EnemyAttackPattern1,
    /// <summary>攻撃(ケケケケ！)</summary>
    EnemyAttackPattern2,
    /// <summary>ダメージ(キーッ!?)</summary>
    EnemyDamagePattern1,
    /// <summary>ダメージ(ギ!?)</summary>
    EnemyDamagePattern2,
    /// <summary>死(ギーッ!)</summary>
    EnemyDeathPattern1,
    /// <summary>死(ヒヒャーッ!)</summary>
    EnemyDeathPattern2,

    /// <summary>チュートリアル開始時(これより、アルシオネ魔法学園の、進級試験を始めます)</summary>
    InstructorTutorialStartPattern1,
    /// <summary>チュートリアル開始時(試験官は私、ミリアが担当するわね)</summary>
    InstructorTutorialStartPattern2,
    /// <summary>チュートリアル開始時(最初はノエリアさんの出番ね、頑張ってちょうだい)</summary>
    InstructorTutorialStartPattern3,
    /// <summary>チュートリアル確認(貴方はとってもイイ感じなんだけど、不安だったらおさらいしておく?)</summary>
    InstructorTutorialCheck,
    /// <summary>チュートリアル確認後(分かったわ。それじゃあまずは)</summary>
    InstructorTutorialOK,
    /// <summary>移動チュートリアル開始(あの辺まで歩いてみてちょうだい)</summary>
    InstructorTutorialMove,
    /// <summary>移動チュートリアル完了(OKよ、次行くわね)</summary>
    InstructorTutorialMoveOK,
    /// <summary>カメラチュートリアル開始(周りを見回してみてちょうだい)</summary>
    InstructorTutorialCamera,
    /// <summary>カメラチュートリアル完了(いい調子よ、このままどんどん行きましょう)</summary>
    InstructorTutorialCameraOK,
    /// <summary>通常攻撃チュートリアル開始(お次は魔法を出してみてちょうだい、えいっと、ね)</summary>
    InstructorTutorialAttack,
    /// <summary>通常攻撃チュートリアル完了(カンタンだったかしら？それじゃあ次よ！)</summary>
    InstructorTutorialAttackOK,
    /// <summary>溜め攻撃チュートリアル開始(沢山1度に攻撃したかったらぐっと溜めてみるといいわよ)</summary>
    InstructorTutorialCharge,
    /// <summary>溜め攻撃チュートリアル完了(凄いわ！バッチリ！)</summary>
    InstructorTutorialChargeOK,
    /// <summary>必殺攻撃チュートリアル開始(敵を攻撃し続けるとトドメを刺せるのよ、もう知ってたかしら？)</summary>
    InstructorTutorialFinish,
    /// <summary>必殺攻撃チュートリアル完了(流石優等生ね！あと少しで先生のお話も終わりよ～)</summary>
    InstructorTutorialFinishOK,
    /// <summary>回避チュートリアル開始(危ない！と思ったら回避を使うのよ！)</summary>
    InstructorTutorialDodge,
    /// <summary>回避チュートリアル完了(うんうん、お見事！私が教えるまでもないわね！)</summary>
    InstructorTutorialDodgeOK,
    /// <summary>ロックオンチュートリアル開始(一体の相手に集中したい時はロックオン！が出来るわ！先に倒したい目標がいるときなんかは便利ね～！)</summary>
    InstructorTutorialLockOn,
    /// <summary>ロックオンチュートリアル完了(呑み込みが早いわ！さらにロックオンの説明を続けるわね～)</summary>
    InstructorTutorialLockOnOK,
    /// <summary>ロックオンの対象変更チュートリアル開始(ロックオンをしている時に対象を切り替えることも出来るわよ～上手に活用してみてね！)</summary>
    InstructorTutorialTargetChange,
    /// <summary>ロックオンの対象変更チュートリアル完了(良いわね～！これを活用できれば戦いやすいと思うわ！)</summary>
    InstructorTutorialTargetChangeOK,
    /// <summary>オプションチュートリアル開始(そうそう、何か困ったことあったら試験は一旦中断出来るわよ！startボタン・・・？っていうのを押せば良いわ～)</summary>
    InstructorTutorialOption,
    /// <summary>オプションチュートリアル完了(音が大きすぎる～って時は利用して頂戴ね！)</summary>
    InstructorTutorialOptionOK,
    /// <summary>ゲームクリア(は～い、試験終了ようふふっ、ノエリアさん、お疲れ様〜)</summary>
    InstructorGameClearPattern1,
    /// <summary>ゲームクリア(そこまで！よく頑張ったわね〜後で先生から合否を送るわね〜)</summary>
    InstructorGameClearPattern2,
    /// <summary>ゲームクリア(終了よ！今すぐ結果を報告したいけど、ちょっと待っててね〜)</summary>
    InstructorGameClearPattern3,
    /// <summary>回復(あらあら、調子が悪かったのかしら？まだまだ頑張れるわ！ファイト〜！)</summary>
    InstructorGameHealPattern1,
    /// <summary>回復(大丈夫？先生の特別サ-ビスよ〜)</summary>
    InstructorGameHealPattern2,
    /// <summary>回復(しっかり！貴方の実力はこんなものじゃないわ～！)</summary>
    InstructorGameHealPattern3,
}