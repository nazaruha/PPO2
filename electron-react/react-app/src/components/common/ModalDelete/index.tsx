import { Modal } from "bootstrap";
import { FC, useRef } from "react";

interface ModalProps {
    id: number,
    text: string,
    deleteFunc: (id: number) => void,
}

const ModalDelete: FC<ModalProps> = ({
    id = -1,
    text = "modalText",
    deleteFunc,
}) => {

    const modalRef = useRef(null);

    const showModal = () => {
        const modal = modalRef.current;
        if (modal) {
            const bsModal = new Modal(modal, {});
            bsModal.show();
        }
    }

    const confirmDelete = () => {
        const modal = modalRef.current;
        if (modal) {
            const bsModal = Modal.getInstance(modal); // посилаєшся на той об'єкт, що був створений при відкритті модалки.
            //Не можна дві подряд модалки створити
            bsModal?.hide();
            deleteFunc(id);
        }
    }

    return (
        <>
            <button className="btn btn-danger" onClick={showModal}>Видалити <i className="fa fa-trash" aria-hidden="true"></i></button>
            <div className="modal fade" ref={modalRef} tabIndex={-1}> {/* tabIndex = -1 щоб при табуляції по об'єктам юзера не табулювало на модалку сховану */}
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Видалення</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <p>{text}</p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Скасувати</button>
                            <button type="button" className="btn btn-danger" onClick={confirmDelete}>Видалити</button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default ModalDelete; // use it here ProductsListPage.tsx