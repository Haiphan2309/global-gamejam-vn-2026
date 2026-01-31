using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Cần thêm thư viện này để dùng List
using System.Linq; // Cần thêm thư viện này để lọc mục tiêu nhanh hơn

public class Fly : MonoBehaviour
{
    [Header("Settings")]
    public float flySpeed = 5f;
    public float eatTime = 3f; // Sửa mặc định thành 3s theo yêu cầu

    private bool isEating = false;
    private bool isDragging = false;
    private Rigidbody2D rb;

    private Vector3 lastPosition;
    private Vector3 throwVelocity;
    private MaskObject targetSlice;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Bắt đầu tìm kiếm ngay khi sinh ra
        TryToFindTarget();
    }

    void TryToFindTarget()
    {
        // Nếu đang có mục tiêu rồi thì không tìm nữa
        if (targetSlice != null) return;

        GameObject container = GameObject.FindGameObjectWithTag("CucumberContainer");
        if (container == null) return;

        // Lấy tất cả các miếng dưa leo trong container
        MaskObject[] allSlices = container.GetComponentsInChildren<MaskObject>();

        // LỌC: Chỉ lấy những miếng chưa bị ai nhắm tới (isTargeted == false)
        // Dùng LINQ cho gọn, hoặc bạn có thể dùng vòng lặp for
        List<MaskObject> availableSlices = allSlices.Where(s => !s.isTargeted && s.itemType == MaskItemType.CucumberSlice).ToList();

        if (availableSlices.Count > 0)
        {
            // Chọn random trong danh sách hợp lệ
            targetSlice = availableSlices[Random.Range(0, availableSlices.Count)];
            targetSlice.isTargeted = true; // Đánh dấu "Miếng này của tao"
        }
    }

    void Update()
    {
        // Nếu bị kéo thì không làm gì cả
        if (isDragging) return;

        // Nếu chưa có mục tiêu (hoặc mục tiêu vừa bị hủy), thử tìm cái mới
        if (targetSlice == null)
        {
            TryToFindTarget();
        }

        // Logic di chuyển: Chỉ bay khi ĐÃ CÓ mục tiêu và KHÔNG ĐANG ĂN
        if (targetSlice != null && !isEating)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetSlice.transform.position, flySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetSlice.transform.position) < 0.1f)
            {
                StartCoroutine(EatRoutine());
            }
        }
    }

    IEnumerator EatRoutine()
    {
        isEating = true; // Dừng di chuyển để đứng lại ăn

        yield return new WaitForSeconds(eatTime); // Chờ 3s

        // Kiểm tra lại xem miếng dưa còn đó không (đề phòng người chơi đã xóa nó trước)
        if (targetSlice != null)
        {
            targetSlice.GetEaten(); // Hủy miếng dưa leo
            targetSlice = null;     // Xóa tham chiếu mục tiêu hiện tại
        }

        // QUAN TRỌNG: Reset trạng thái để bay tiếp
        isEating = false; 
        
        // Ngay lập tức tìm mục tiêu mới
        TryToFindTarget();
    }

    // --- CÁC HÀM XỬ LÝ CHUỘT (GIỮ NGUYÊN) ---

    void OnMouseDown()
    {
        isDragging = true;
        lastPosition = transform.position;
        StopAllCoroutines(); // Dừng ăn ngay lập tức nếu bị tóm
        isEating = false; // Reset trạng thái ăn
        
        // Nếu đang nhắm miếng nào thì thả miếng đó ra (để con ruồi khác có thể đến ăn)
        if (targetSlice != null) 
        {
            targetSlice.isTargeted = false; 
            targetSlice = null;
        }
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        throwVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.velocity = throwVelocity * 0.5f;
        rb.angularVelocity = 720f;
        Destroy(gameObject, 2f);
    }
}