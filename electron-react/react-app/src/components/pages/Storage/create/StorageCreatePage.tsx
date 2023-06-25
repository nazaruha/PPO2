import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { IManufacturerItem, IManufacturerServiceResponse } from "../../Manufacturer/types";
import { IStorageCreateItem } from "../types";
import { useFormik } from "formik";
import { StorageCreateSchema } from "../validation";
import classNames from "classnames";
import http from "../../../../http_common";
import { IProductItem } from "../../Product/types";

const StorageCreatePage = () => {
    const navigate = useNavigate();

    const projectId = localStorage.chosenProjectId;
    const [manufacturers, setManufacturers] = useState<IManufacturerItem[]>([]);
    const [products, setProducts] = useState<IProductItem[]>([]);


    const init: IStorageCreateItem = {
        productId: 0,
        projectId: projectId,
        price: 0,
        count: 0,
        description: "",
        manufacturerId: 0,
        expireDate: "",
    }

    const onCreateSubmit = (values: IStorageCreateItem) => {
        console.log("VALUES", values);
        http.post("api/storage/create", values)
            .then(resp => {
                console.log("RESP", resp);
                navigate("..");
            })
            .catch(err => {
                console.log("ERR", err);
                const { data } = err.response;
                if (data.includes("Цей продукт")) {
                    setFieldError("productId", data);
                    setFieldError("manufacturerId", data);
                }
            })
    }

    const formik = useFormik({
        initialValues: init,
        onSubmit: onCreateSubmit,
        validationSchema: StorageCreateSchema
    })

    const { errors, values, touched, handleChange, handleSubmit, setFieldError } = formik;

    useEffect(() => {
        http.get<IManufacturerServiceResponse>("api/manufacturer/index")
            .then(resp => {
                const { payload } = resp.data;
                console.log("MANUFACTURER LIST", resp);
                setManufacturers(payload as IManufacturerItem[]);
                console.log("ManufacturerId", values.manufacturerId);
                if (values.manufacturerId !== 0) {
                    http.get<IProductItem[]>(`api/product/manufacturerId-${values.manufacturerId}`)
                        .then(resp => {
                            console.log("PRODUCTS BY MANUFACTURER ID", resp);
                            setProducts(resp.data);
                        })
                        .catch(err => {
                            console.log("ERR PRODUCTSLST", err);
                        })
                }

            })
            .catch(err => {
                console.log("ERR MANLST", err);
            });
    }, [values.manufacturerId])


    return (
        <>
            <h1 className="text-center">Сховище - добавлення продукту</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3 row">
                    <div className="col-md-6">
                        <label htmlFor="manufacturerId" className='form-label'>Оберіть Виробника</label>
                        <select
                            className={classNames(
                                "form-select",
                                { "is-invalid": errors.manufacturerId && touched.manufacturerId }
                            )}
                            aria-label="Manufacturer select"
                            defaultValue={values.manufacturerId} // Значення, яке міститься в select
                            onChange={handleChange} // Якщо значення міняється, воно записується у формік
                            name="manufacturerId" // Значення поля у форміку = manufacturerId - якщо його не буде - formik - не буде нічого добавлять
                            id='manufacturerId' // Це використовується для label
                        >
                            {/* Значення, яке завжди буде не обране, для того, щоб нагадать, що треба вказувать категорію */}
                            <option value="0" disabled>---</option> {/* disabled - щоб при виборі категорії ти не міг обрати цей варік */}
                            {/* Перебираємо список категорій і виводимо їх у вигляді options */}
                            {manufacturers.map(item => (
                                <option key={item.id} value={item.id}>{item.name}</option>
                            ))}
                        </select>
                        {(errors.manufacturerId && touched.manufacturerId) && (
                            <span className='text-danger'>{errors.manufacturerId}</span>
                        )}
                    </div>
                    <div className="col-md-6">
                        <label htmlFor="productId" className='form-label'>Оберіть Продукт</label>
                        <select
                            className={classNames(
                                "form-select",
                                { "is-invalid": errors.productId && touched.productId }
                            )}
                            aria-label="Product select"
                            defaultValue={values.productId} // Значення, яке міститься в select
                            onChange={handleChange} // Якщо значення міняється, воно записується у формік
                            name="productId" // Значення поля у форміку = manufacturerId - якщо його не буде - formik - не буде нічого добавлять
                            id='productId' // Це використовується для label
                        >
                            {/* Значення, яке завжди буде не обране, для того, щоб нагадать, що треба вказувать категорію */}
                            <option value="0" disabled>---</option> {/* disabled - щоб при виборі категорії ти не міг обрати цей варік */}
                            {/* Перебираємо список категорій і виводимо їх у вигляді options */}
                            {products.map(item => (
                                <option key={item.id} value={item.id}>{item.name}</option>
                            ))}
                        </select>
                        {(errors.productId && touched.productId) && (
                            <span className='text-danger'>{errors.productId}</span>
                        )}
                    </div>
                </div>
                <div className="mb-3">
                    <label htmlFor="description" className="form-label">Опис</label>
                    <input type="text" id="description" name="description" className={classNames(
                        "form-control",
                        { "is-invalid": errors.description && touched.description }
                    )} value={values.description} onChange={handleChange} placeholder="Введіть опис" />
                    {errors.description && touched.description && (
                        <div className="invalid-feedback">
                            {errors.description}
                        </div>
                    )}
                </div>
                <div className="mb-3 input-group">
                    <label htmlFor="price" className="form-label">Ціна</label>
                    <div className="input-group">
                        <input type="number" id="price" name="price" className={classNames(
                            "form-control",
                            { "is-invalid": errors.price && touched.price }
                        )} value={values.price} onChange={handleChange} placeholder="Введіть ціну" />
                        <span className="input-group-text">₴</span>
                        {errors.price && touched.price && (
                            <div className="invalid-feedback">
                                {errors.price}
                            </div>
                        )}
                    </div>
                </div>
                <div className="mb-3">
                    <label htmlFor="count" className="form-label">Кількість</label>
                    <input type="number" id="count" name="count" className={classNames(
                        "form-control",
                        { "is-invalid": errors.count && touched.count }
                    )} value={values.count} onChange={handleChange} placeholder="Введіть кількість" />
                    {errors.count && touched.count && (
                        <div className="invalid-feedback">
                            {errors.count}
                        </div>
                    )}
                </div>

                <div className="mb-3">
                    <label htmlFor="expireDate" className="form-label">Термін придатності</label>
                    <input
                        type="datetime-local"
                        className={classNames(
                            "form-control",
                            { "is-invalid": errors.expireDate && touched.expireDate }
                        )}
                        id="expireDate"
                        name="expireDate"
                        onChange={handleChange}
                        value={values.expireDate.toLocaleString()}
                    />
                    {errors.expireDate && touched.expireDate && (
                        <div className="invalid-feedback">
                            {errors.expireDate}
                        </div>
                    )}
                </div>

                <div className="container d-flex flex-row justify-content-center gap-3">
                    <button type="submit" className="btn btn-primary">Створити</button>
                    <Link to="../" className="btn btn-warning">Відхілити</Link>
                </div>
            </form>
        </>
    )
}

export default StorageCreatePage;