export interface ModalFrameCallback<T> {
    onOk?: (selectedValue?: T) => void;
    onCancel?: (selectedValue?: T) => void;
}