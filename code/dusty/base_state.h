// Base state class for the game, used for the state machine of the character and other objects in the game
UCLASS()
class DUSTY_API UBaseState : public UObject
{
    GENERATED_BODY()

public:
    UBaseState() {};
    virtual ~UBaseState();

    virtual void Init() {};
    virtual void Update(float _DeltaTime) {};
    virtual void Exit() {};

    void SetOwner(AActor* InOwner) { Owner = InOwner; }
    bool CanUpdateTick = false;

protected:
    UPROPERTY(Transient)
    TObjectPtr<AActor> Owner = nullptr;

    template<typename T>
    T* GetOwnerAs() const
    {
        return Cast<T>(Owner);
    }
};

// Character base state
UCLASS()
class DUSTY_API UCharacterBase : public UBaseState
{
    GENERATED_BODY()

public:
    virtual void Init() {};
    virtual void Update(float _DeltaTime) {};
    virtual void Exit() {};

protected:
    ABaseCharacter* GetCharacter() const
    {
        return GetOwnerAs<ABaseCharacter>();
    };
};


// Ground movement state for the character
UCLASS()
class DUSTY_API UGroundMovementState : public UCharacterBase
{
    GENERATED_BODY()

public:
    virtual void Init() {};
    virtual void Update(float _DeltaTime) {};
    virtual void Exit() {};
};