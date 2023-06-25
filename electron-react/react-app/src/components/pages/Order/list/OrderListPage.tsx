import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { IOrderSearch, IOrderSearchResult } from "../types";
import { useFormik } from "formik";
import http from "../../../../http_common";
import classNames from "classnames";
import ModalDelete2 from "../../../common/ModalDelete2";

const OrderListPage = () => {
    const [searchParams, setSearchParams] = useSearchParams();

    const projectId = localStorage.chosenProjectId;
    const [search, setSearch] = useState<IOrderSearch>({
        customerFirstName: searchParams.get("customerFirstName") || "",
        customerSecondName: searchParams.get("customerSecondName") || "",
        productName: searchParams.get("productName") || "",
        manufacturerName: searchParams.get("manufacturerName") || "",
        totalPrice: searchParams.get("totalPrice") || 0,
        productQuantity: searchParams.get("productQuantity") || 0,
        projectId: searchParams.get("projectId") || Number(projectId),
        id: searchParams.get("id") || 0,
        page: searchParams.get("page") || 1,
    });

    const onSubmitSearch = (values: IOrderSearch) => {
        console.log("Проводимо пошук", values);
        setSearch(values);
    }

    const formik = useFormik({
        initialValues: search,
        onSubmit: onSubmitSearch
    });

    const { values, touched, errors, handleSubmit, handleChange } = formik;

    const [searchResult, setSearchResult] = useState<IOrderSearchResult>({
        orders: [],
        pages: 0,
        currentPage: 0,
        total: 0,
    });

    useEffect(() => {
        http.get<IOrderSearchResult>(`api/order/search`, {
            params: search
        })
            .then(resp => {
                const { data } = resp;
                console.log("Server response", data);
                setSearchResult(data);
            }).catch(err => {
                console.log("ERR", err);
            })
    }, [search])

    const { orders, pages, currentPage } = searchResult;

    const buttons = [];
    for (let i = 1; i <= pages; i++) {
        buttons.push(i);
    }

    const pagination = buttons.map(x => {
        return (
            <li className={classNames(
                "page-item",
                { "active": x === currentPage }
            )} key={x}>
                <Link className="page-link" to={`?page=${x}`} onClick={() => setSearch({ ...search, page: x })}>
                    {x}
                </Link>
            </li>
        );
    })

    const onDeleteOrder = async (orderId: number, projectId: number) => {
        try {
            await http.delete(`api/order/delete/orderId-${orderId}/projectId-${projectId}`);
            setSearch({ ...search, page: 1 });
        } catch (err) {
            console.log("ERR DELETE", err);
        }
    }

    const tableViewList = orders.map((item) => {
        return (
            <tr key={item.id}>
                <th scope="row">{item.id}</th>
                <td>{item.customer.firstName}&nbsp;{item.customer.secondName}</td>
                <td>{item.customer.email} <b>|</b> {item.customer.phone}</td>
                <td>{item.product.name}</td>
                <td>{item.product.manufacturerName}</td>
                <td>{item.productQuantity}</td>
                <td>{item.totalPrice}</td>
                <td>{item.sellDate.toLocaleString()}</td>
                <td style={{ display: 'flex' }}>
                    <Link to={`edit/${item.id}`} className="btn btn-warning"
                    >
                        Змінити <i className="fa fa-pencil" aria-hidden="true"></i>
                    </Link>
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    <ModalDelete2
                        childId={item.id}
                        parentId={projectId}
                        text={`Ви дійсно бажаєте видалити замовлення під номером ${item.id}?`}
                        deleteFunc={onDeleteOrder}
                    />
                </td>
            </tr>
        )
    })



    return (
        <>
            <h1 className="text-center">Замовлення</h1>

            <Link to="create" className="btn btn-success fs-5">Додати <i className="fa fa-plus-square-o fs-6" aria-hidden="true"></i></Link>
            <table className="table text-center">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Користувач</th>
                        <th scope="col">Пошта | Телефон</th>
                        <th scope="col">Продукт</th>
                        <th scope="col">Виробник</th>
                        <th scope="col">Кількість</th>
                        <th scope="col">Сумма ₴</th>
                        <th scope="col">Дата продажу</th>
                        <th style={{ width: '20%' }}></th>
                    </tr>
                </thead>
                <tbody>
                    {tableViewList}
                </tbody>
            </table>

            <nav aria-label="..." style={{ display: 'flex', justifyContent: 'end' }}>
                <ul className="pagination">
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == 1 }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) - 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) - 1 })} className="fa fa-arrow-left text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                    {pagination}
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == buttons.length }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) + 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) + 1 })} className="fa fa-arrow-right text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                </ul>
            </nav>
        </>
    )
}

export default OrderListPage;